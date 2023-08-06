using BaseCore.Extensions;
using EmailCore.Declarations.Repositories;
using EmailCore.Declarations.Services;
using EmailCore.Models.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailCore.Implementations.Services
{
    public class EmailSenderHostedService : BackgroundService
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogger<EmailSenderHostedService> _logger;
        private readonly IEmailMessageRepository _emailMessageRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public EmailSenderHostedService(
            IEmailSenderService emailSenderService,
            ILogger<EmailSenderHostedService> logger,
            IEmailMessageRepository emailMessageRepository,
            IEmailTemplateRepository emailTemplateRepository)
        {
            _emailSenderService = emailSenderService;
            _logger = logger;
            _emailMessageRepository = emailMessageRepository;
            _emailTemplateRepository = emailTemplateRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Fetch email messages from the database
                    var emailMessages = await GetEmailMessagesFromDatabase();

                    foreach (var emailMessage in emailMessages)
                    {
                        // Send the email using the EmailSender service

                        EmailTemplate emailTemplate = null;

                        if (!emailMessage.EmailTemplateId.IsNullOrBlank())
                        {
                            emailTemplate = await _emailTemplateRepository.GetEmailTemplate(emailMessage.EmailTemplateId);
                        }

                        var result = await _emailSenderService.SendEmailAsync(emailMessage, emailTemplate);

                        if (result)
                        {
                            // TODO: update email message as status  = sent
                        }
                        else
                        {
                            // TODO: update email message as status  = failed and retry count ++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions here, for example, log the error

                    _logger.LogError(ex.Message, ex);
                }

                // Wait for some time before checking for new emails again
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task<EmailMessage[]> GetEmailMessagesFromDatabase()
        {
            var emailMessages = await _emailMessageRepository.GetEmailMessagesForSending();

            return emailMessages;
        }
    }
}
