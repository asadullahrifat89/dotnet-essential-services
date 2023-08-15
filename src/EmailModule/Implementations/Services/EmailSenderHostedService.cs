using BaseModule.Infrastructure.Extensions;
using EmailModule.Declarations.Repositories;
using EmailModule.Declarations.Services;
using EmailModule.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailModule.Implementations.Services
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
                try
                {
                    var batch = new List<(EmailMessage? EmailMessage, EmailTemplate? EmailTemplate)>();

                    // Fetch email messages from the database
                    var emailMessages = await GetEmailMessagesFromDatabase();

                    // Prepare a batch of emails to be sent
                    foreach (var emailMessage in emailMessages)
                    {
                        EmailTemplate? emailTemplate = null;

                        if (!emailMessage.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank())
                        {
                            emailTemplate = await _emailTemplateRepository.GetEmailTemplate(emailMessage.EmailTemplateConfiguration.EmailTemplateId);
                        }

                        batch.Add((emailMessage, emailTemplate));
                    }

                    if (batch.Any())
                    {
                        // Send the email batch
                        var result = await _emailSenderService.SendEmailBatch(batch, async (result) =>
                        {
                            if (result.IsSuccess)
                            {
                                await _emailMessageRepository.UpdateEmailMessageStatus(result.EmailMessage.Id, EmailSendStatus.Sent);
                            }
                            else
                            {
                                await _emailMessageRepository.UpdateEmailMessageStatus(result.EmailMessage.Id, EmailSendStatus.Failed);
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
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
