using EmailCore.Declarations.Services;
using EmailCore.Models.Entities;
using MimeKit;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmailCore.Implementations.Services
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

        public async Task<bool> SendEmailAsync(EmailMessage emailMessage, EmailTemplate emailTemplate)
        {
            try
            {
                using (var mimeMessage = new MimeMessage())
                {
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

                    switch (emailTemplate.EmailTemplateType)
                    {
                        case EmailTemplateType.Text:
                            emailBodyBuilder.TextBody = emailMessage.Body;
                            break;
                        case EmailTemplateType.HTML:
                            emailBodyBuilder.HtmlBody = emailMessage.Body;
                            break;
                        default:
                            break;
                    }

                    mimeMessage.Body = emailBodyBuilder.ToMessageBody();

                    var server = _configuration["MailSettings:Server"];
                    var port = _configuration["MailSettings:Port"];
                    var userName = _configuration["MailSettings:UserName"];
                    var password = _configuration["MailSettings:Password"];

                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (var mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        await mailClient.ConnectAsync(server, Convert.ToInt32(port), MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(userName, password);
                        await mailClient.SendAsync(mimeMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }
    }
}
