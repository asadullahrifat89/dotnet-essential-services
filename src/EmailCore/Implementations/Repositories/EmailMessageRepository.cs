using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Repositories;
using EmailCore.Models.Entities;
using Microsoft.Extensions.Configuration;

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

        public EmailMessageRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContext, IConfiguration configuration, IEmailTemplateRepository emailTemplateRepository)
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

            if (!emailMessage.EmailTemplateId.IsNullOrBlank())
            {
                var emailTemplate = await _emailTemplateRepository.GetEmailTemplate(emailMessage.EmailTemplateId);

                var body = emailMessage.Body;

                foreach (var tag in emailTemplate.Tags)
                {
                    var sourceTag = "{" + $"{tag}" + "}";

                    if (body.Contains(sourceTag) && emailMessage.TagValues.ContainsKey(tag))
                        body = body.Replace(sourceTag, emailMessage.TagValues[tag]);
                }
            }

            await _mongoDbService.InsertDocument(emailMessage);

            return Response.BuildServiceResponse().BuildSuccessResponse(emailMessage, authCtx?.RequestUri);
        }

        public Task<EmailMessage[]> GetEmailMessagesForSending()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}