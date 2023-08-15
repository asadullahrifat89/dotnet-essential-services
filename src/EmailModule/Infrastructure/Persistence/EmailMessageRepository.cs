﻿using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using BaseModule.Infrastructure.Extensions;
using EmailModule.Application.Commands;
using EmailModule.Domain.Entities;
using EmailModule.Domain.Repositories.Interfaces;
using IdentityModule.Application.Providers.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Net;

namespace EmailModule.Infrastructure.Persistence
{
    public class EmailMessageRepository : IEmailMessageRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly IConfiguration _configuration;
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        #endregion

        #region Ctor

        public EmailMessageRepository(
            IMongoDbContextProvider mongoDbService,
            IAuthenticationContextProvider authenticationContext,
            IConfiguration configuration,
            IEmailTemplateRepository emailTemplateRepository)
        {
            _mongoDbContextProvider = mongoDbService;
            _authenticationContextProvider = authenticationContext;
            _configuration = configuration;
            _emailTemplateRepository = emailTemplateRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> EnqueueEmailMessage(EmailMessage emailMessage)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var senderName = _configuration["MailSettings:SenderName"];
            var senderEmail = _configuration["MailSettings:SenderEmail"];

            emailMessage.From = new EmailContact()
            {
                Email = senderEmail,
                Name = senderName,
            };

            // replace tags

            switch (emailMessage.EmailBodyType)
            {
                case EmailBodyType.NonTemplated:
                    {
                        var body = emailMessage.EmailBody.Content;

                        switch (emailMessage.EmailBody.EmailBodyContentType)
                        {
                            case EmailBodyContentType.Text:
                                {
                                    emailMessage.EmailBody.Content = body;
                                }
                                break;
                            case EmailBodyContentType.HTML:
                                {
                                    string decodedHtml = WebUtility.HtmlDecode(body);
                                    emailMessage.EmailBody.Content = decodedHtml;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case EmailBodyType.Templated:
                    {
                        if (!emailMessage.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank())
                        {
                            var emailTemplate = await _emailTemplateRepository.GetEmailTemplateById(emailMessage.EmailTemplateConfiguration.EmailTemplateId);

                            var body = emailTemplate.Body;

                            foreach (var tag in emailTemplate.Tags)
                            {
                                var sourceTag = "{" + $"{tag}" + "}";

                                if (body.Contains(sourceTag) && emailMessage.EmailTemplateConfiguration.TagValues.ContainsKey(tag))
                                    body = body.Replace(sourceTag, emailMessage.EmailTemplateConfiguration.TagValues[tag]);
                            }

                            switch (emailTemplate.EmailBodyContentType)
                            {
                                case EmailBodyContentType.Text:
                                    {
                                        emailMessage.EmailBody.Content = body;
                                    }
                                    break;
                                case EmailBodyContentType.HTML:
                                    {
                                        string decodedHtml = WebUtility.HtmlDecode(body);
                                        emailMessage.EmailBody.Content = decodedHtml;
                                    }
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

            await _mongoDbContextProvider.InsertDocument(emailMessage);

            return Response.BuildServiceResponse().BuildSuccessResponse(emailMessage, authCtx?.RequestUri);
        }

        public async Task<List<EmailMessage>> GetEmailMessagesForSending()
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var retryThreshold = Convert.ToInt32(_configuration["MailSettings:RetryThreshold"]);

            var filter = Builders<EmailMessage>.Filter.In(x => x.EmailSendStatus, new EmailSendStatus[] { EmailSendStatus.Pending, EmailSendStatus.Failed });

            if (retryThreshold > 0)
                filter &= Builders<EmailMessage>.Filter.Lt(x => x.SendingAttempt, retryThreshold);

            var emailMessages = await _mongoDbContextProvider.GetDocuments(
                filter: filter,
                skip: 0,
                limit: 10,
                sortOrder: SortOrder.Descending,
                sortFieldName: "EmailSendStatus");

            return emailMessages;
        }

        public async Task<bool> UpdateEmailMessageStatus(string emailMessageId, EmailSendStatus emailSendStatus)
        {
            var update = emailSendStatus == EmailSendStatus.Failed
                ? Builders<EmailMessage>.Update.Set(x => x.EmailSendStatus, emailSendStatus).Inc(x => x.SendingAttempt, 1)
                : Builders<EmailMessage>.Update.Set(x => x.EmailSendStatus, emailSendStatus);

            await _mongoDbContextProvider.UpdateById(update: update, id: emailMessageId);

            return true;
        }

        #endregion
    }
}