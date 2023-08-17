using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Email.Application.Services.Interfaces;
using Email.Domain.Entities;
using Base.Application.Extensions;

namespace Email.Application.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSenderService> _logger;

        public EmailSenderService(IConfiguration configuration, ILogger<EmailSenderService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmail(EmailMessage emailMessage, EmailTemplate emailTemplate)
        {
            var server = _configuration["MailSettings:Server"];
            var port = _configuration["MailSettings:Port"];
            var userName = _configuration["MailSettings:UserName"];
            var password = _configuration["MailSettings:Password"];

            using var mailClient = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                // establish connection
                await mailClient.ConnectAsync(server, Convert.ToInt32(port), MailKit.Security.SecureSocketOptions.StartTls);
                await mailClient.AuthenticateAsync(userName, password);

                // prepare mime message
                using MimeMessage mimeMessage = PrepareMimeMessage(emailMessage, emailTemplate);

                // send mime message
                await mailClient.SendAsync(mimeMessage);

                // disconnect connection
                await mailClient.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                if (mailClient is not null && mailClient.IsConnected)
                    await mailClient.DisconnectAsync(true);

                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> SendEmailBatch(List<(EmailMessage? EmailMessage, EmailTemplate? EmailTemplate)> emailBatch, Action<(bool IsSuccess, EmailMessage? EmailMessage)> onBatchItemProcess)
        {
            var server = _configuration["MailSettings:Server"];
            var port = _configuration["MailSettings:Port"];
            var userName = _configuration["MailSettings:UserName"];
            var password = _configuration["MailSettings:Password"];

            var emailMessageInProcess = new EmailMessage();

            using var mailClient = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                // establish connection
                await mailClient.ConnectAsync(server, Convert.ToInt32(port), MailKit.Security.SecureSocketOptions.StartTls);
                await mailClient.AuthenticateAsync(userName, password);

                foreach (var email in emailBatch)
                {
                    var emailMessage = email.EmailMessage;
                    var emailTemplate = email.EmailTemplate;

                    emailMessageInProcess = emailMessage;

                    // send mime message
                    using MimeMessage mimeMessage = PrepareMimeMessage(emailMessage, emailTemplate);
                    await mailClient.SendAsync(mimeMessage);

                    onBatchItemProcess?.Invoke((true, emailMessageInProcess));
                }

                // disconnect connection
                await mailClient.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                onBatchItemProcess?.Invoke((false, emailMessageInProcess));

                if (mailClient is not null && mailClient.IsConnected)
                    await mailClient.DisconnectAsync(true);

                return false;
            }
        }

        private MimeMessage PrepareMimeMessage(EmailMessage? emailMessage, EmailTemplate? emailTemplate)
        {
            // if email template configuration and email template id is provided in the email message but the actual email template is not found while sending this email, throw exception
            if (emailMessage.EmailTemplateConfiguration is not null
                && !emailMessage.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank()
                && emailTemplate is null)
            {
                throw new Exception($"Email template not found by id: {emailMessage.EmailTemplateConfiguration.EmailTemplateId}.");
            }

            var mimeMessage = new MimeMessage();
            var emailFrom = new MailboxAddress(emailMessage.From.Name, emailMessage.From.Email);
            mimeMessage.From.Add(emailFrom);

            foreach (var to in emailMessage.To)
            {
                var emailTo = new MailboxAddress(to.Name, to.Email);
                mimeMessage.To.Add(emailTo);
            }

            foreach (var cc in emailMessage.CC)
            {
                var emailTo = new MailboxAddress(cc.Name, cc.Email);
                mimeMessage.Cc.Add(emailTo);
            }

            foreach (var bcc in emailMessage.BCC)
            {
                var emailTo = new MailboxAddress(bcc.Name, bcc.Email);
                mimeMessage.Bcc.Add(emailTo);
            }

            mimeMessage.Subject = emailMessage.Subject;

            var emailBodyBuilder = new BodyBuilder();

            switch (emailMessage.EmailBodyType)
            {
                case EmailBodyType.NonTemplated:
                    {
                        switch (emailMessage.EmailBody.EmailBodyContentType)
                        {
                            case EmailBodyContentType.Text:
                                emailBodyBuilder.TextBody = emailMessage.EmailBody.Content;
                                break;
                            case EmailBodyContentType.HTML:
                                emailBodyBuilder.HtmlBody = emailMessage.EmailBody.Content;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case EmailBodyType.Templated:
                    {
                        if (emailTemplate is not null)
                        {
                            switch (emailTemplate.EmailBodyContentType)
                            {
                                case EmailBodyContentType.Text:
                                    emailBodyBuilder.TextBody = emailMessage.EmailBody.Content;
                                    break;
                                case EmailBodyContentType.HTML:
                                    emailBodyBuilder.HtmlBody = emailMessage.EmailBody.Content;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            mimeMessage.Body = emailBodyBuilder.ToMessageBody();
            return mimeMessage;
        }
    }
}
