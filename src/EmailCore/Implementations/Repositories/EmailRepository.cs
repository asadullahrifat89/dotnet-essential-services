using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using BaseCore.Services;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Queries;
using EmailCore.Declarations.Repositories;
using EmailCore.Models.Entities;
using MongoDB.Driver;



namespace EmailCore.Implementations.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public EmailRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }


        #endregion

        #region Methods

        public async Task<ServiceResponse> CreateTemplate(CreateTemplateCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var template = EmailTemplate.Initialize(command, authCtx);


            await _mongoDbService.InsertDocument(template);

            return Response.BuildServiceResponse().BuildSuccessResponse(template, authCtx?.RequestUri);
        }

        public async Task<QueryRecordResponse<EmailTemplate>> GetEmailTemplate(GetEmailTemplateQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Id, query.TemplateId);

            var emailTemplate = await _mongoDbService.FindOne(filter);

            return emailTemplate == null ? Response.BuildQueryRecordResponse<EmailTemplate>().BuildErrorResponse(new ErrorResponse().BuildExternalError("Template doesn't exist."), authCtx?.RequestUri) : Response.BuildQueryRecordResponse<EmailTemplate>().BuildSuccessResponse(emailTemplate, authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingEmailTemplate(string templateName)
        {
            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Name, templateName); return await _mongoDbService.Exists<EmailTemplate>(filter);
        }

        public async Task<bool> BeAnExistingEmailTemplateById(string templateId)
        {
            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Id, templateId);

            return await _mongoDbService.Exists<EmailTemplate>(filter);
        }
        #endregion




    }
}
