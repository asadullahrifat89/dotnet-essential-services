using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Repositories;
using EmailCore.Models.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Net;

namespace EmailCore.Implementations.Repositories
{
    public class EmailMessageRepository : IEmailMessageRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        #endregion

        #region Ctor

        public EmailMessageRepository(
            IMongoDbService mongoDbService,
            IAuthenticationContextProvider authenticationContext,
            IConfiguration configuration,
            IEmailTemplateRepository emailTemplateRepository)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
            _configuration = configuration;
            _emailTemplateRepository = emailTemplateRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> EnqueueEmailMessage(EnqueueEmailMessageCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var emailMessage = EmailMessage.Initialize(command, authCtx);

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
                        if (!emailMessage.EmailTemplateId.IsNullOrBlank())
                        {
                            var emailTemplate = await _emailTemplateRepository.GetEmailTemplate(emailMessage.EmailTemplateId);

                            var body = emailTemplate.Body;

                            foreach (var tag in emailTemplate.Tags)
                            {
                                var sourceTag = "{" + $"{tag}" + "}";

                                if (body.Contains(sourceTag) && emailMessage.TagValues.ContainsKey(tag))
                                    body = body.Replace(sourceTag, emailMessage.TagValues[tag]);
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

            await _mongoDbService.InsertDocument(emailMessage);

            return Response.BuildServiceResponse().BuildSuccessResponse(emailMessage, authCtx?.RequestUri);
        }

        public async Task<List<EmailMessage>> GetEmailMessagesForSending()
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var retryThreshold = Convert.ToInt32(_configuration["MailSettings:RetryThreshold"]);

            var filter = Builders<EmailMessage>.Filter.In(x => x.EmailSendStatus, new EmailSendStatus[] { EmailSendStatus.Pending, EmailSendStatus.Failed });

            if (retryThreshold > 0)
                filter &= Builders<EmailMessage>.Filter.Lt(x => x.SendingAttempt, retryThreshold);

            var emailMessages = await _mongoDbService.GetDocuments(filter: filter, skip: 0, limit: 10, sortOrder: SortOrder.Descending, sortFieldName: "EmailSendStatus");

            return emailMessages;
        }

        public async Task<bool> UpdateEmailMessageStatus(string emailMessageId, EmailSendStatus emailSendStatus)
        {
            var update = emailSendStatus == EmailSendStatus.Failed
                ? Builders<EmailMessage>.Update.Set(x => x.EmailSendStatus, emailSendStatus).Inc(x => x.SendingAttempt, 1)
                : Builders<EmailMessage>.Update.Set(x => x.EmailSendStatus, emailSendStatus);

            await _mongoDbService.UpdateById(update: update, id: emailMessageId);

            return true;
        }

        #endregion
    }
}