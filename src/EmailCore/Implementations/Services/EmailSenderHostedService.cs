using BaseCore.Extensions;
using EmailCore.Declarations.Repositories;
using EmailCore.Declarations.Services;
using EmailCore.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailCore.Implementations.Services
{
    public class EmailSenderHostedService : BackgroundService
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogger<EmailSenderHostedService> _logger;
        private readonly IEmailMessageRepository _emailMessageRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly int _enqueuedEmailMessageProcessingIntervalInSeconds;

        public EmailSenderHostedService(
            IEmailSenderService emailSenderService,
            ILogger<EmailSenderHostedService> logger,
            IEmailMessageRepository emailMessageRepository,
            IEmailTemplateRepository emailTemplateRepository,
            IConfiguration configuration)
        {
            _emailSenderService = emailSenderService;
            _logger = logger;
            _emailMessageRepository = emailMessageRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _enqueuedEmailMessageProcessingIntervalInSeconds = Convert.ToInt32(configuration["MailSettings:EnqueuedEmailMessageProcessingIntervalInSeconds"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                EmailMessage emailMessageinProcess = null;

                try
                {
                    // Fetch email messages from the database
                    var emailMessages = await GetEmailMessagesFromDatabase();

                    foreach (var emailMessage in emailMessages)
                    {
                        // Send the email using the EmailSender service

                        emailMessageinProcess = emailMessage;

                        EmailTemplate emailTemplate = null;

                        if (!emailMessage.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank())
                        {
                            emailTemplate = await _emailTemplateRepository.GetEmailTemplate(emailMessage.EmailTemplateConfiguration.EmailTemplateId);
                        }

                        var result = await _emailSenderService.SendEmailAsync(emailMessage, emailTemplate);

                        if (result)
                        {
                            await _emailMessageRepository.UpdateEmailMessageStatus(emailMessage.Id, EmailSendStatus.Sent);
                        }
                        else
                        {
                            await _emailMessageRepository.UpdateEmailMessageStatus(emailMessage.Id, EmailSendStatus.Failed);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions here, for example, log the error
                    if (emailMessageinProcess is not null)
                        await _emailMessageRepository.UpdateEmailMessageStatus(emailMessageinProcess.Id, EmailSendStatus.Failed);

                    _logger.LogError(ex.Message, ex);
                }

                // Wait for some time before checking for new emails again
                await Task.Delay(TimeSpan.FromSeconds(_enqueuedEmailMessageProcessingIntervalInSeconds), stoppingToken);
            }
        }

        private async Task<List<EmailMessage>> GetEmailMessagesFromDatabase()
        {
            var emailMessages = await _emailMessageRepository.GetEmailMessagesForSending();

            return emailMessages;
        }
    }
}
