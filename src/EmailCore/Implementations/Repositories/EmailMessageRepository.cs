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

        #endregion

        #region Ctor

        public EmailMessageRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContext, IConfiguration configuration)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
            _configuration = configuration;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> EnqueueEmailMessage(EnqueueEmailMessageCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var message = EmailMessage.Initialize(command, authCtx);

            var senderName = _configuration["MailSettings:SenderName"];
            var senderEmail = _configuration["MailSettings:SenderEmail"];

            message.From = new EmailContact()
            {
                Email = senderEmail,
                Name = senderName
            };

            await _mongoDbService.InsertDocument(message);

            return Response.BuildServiceResponse().BuildSuccessResponse(message, authCtx?.RequestUri);
        }

        #endregion
    }
}