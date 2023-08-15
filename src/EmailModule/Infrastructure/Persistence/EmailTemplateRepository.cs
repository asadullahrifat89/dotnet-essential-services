using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using EmailModule.Application.Commands;
using EmailModule.Application.Queries;
using EmailModule.Domain.Entities;
using EmailModule.Domain.Interfaces;
using IdentityModule.Infrastructure.Services.Interfaces;
using MongoDB.Driver;


namespace EmailModule.Infrastructure.Persistence
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProviderService _authenticationContext;

        #endregion

        #region Ctor

        public EmailTemplateRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProviderService authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }


        #endregion

        #region Methods

        public async Task<ServiceResponse> CreateEmailTemplate(CreateEmailTemplateCommand command)
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

            return Response.BuildQueryRecordResponse<EmailTemplate>().BuildSuccessResponse(emailTemplate, authCtx?.RequestUri);
        }

        public async Task<ServiceResponse> UpdateEmailTemplate(UpdateEmailTemplateCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var update = Builders<EmailTemplate>.Update
                .Set(x => x.Name, command.Name)
                .Set(x => x.Body, command.Body)
                .Set(x => x.EmailBodyContentType, command.EmailBodyContentType)
                .Set(x => x.Purpose, command.Purpose)
                .Set(x => x.Tags, command.Tags)
                .Set(x => x.TimeStamp.ModifiedOn, DateTime.UtcNow)
                .Set(x => x.TimeStamp.ModifiedBy, authCtx.User?.Id);

            await _mongoDbService.UpdateById(update: update, id: command.TemplateId);

            var updatedTemplate = await _mongoDbService.FindById<EmailTemplate>(command.TemplateId);

            return Response.BuildServiceResponse().BuildSuccessResponse(updatedTemplate, authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingEmailTemplate(string templateName)
        {
            var filter = Builders<EmailTemplate>.Filter.Where(x => x.Name.ToLower().Equals(templateName.ToLower()));
            return await _mongoDbService.Exists(filter);
        }

        public async Task<bool> BeAnExistingEmailTemplateById(string templateId)
        {
            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Id, templateId);
            return await _mongoDbService.Exists(filter);
        }

        public async Task<EmailTemplate> GetEmailTemplate(string templateId)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Id, templateId);

            var emailTemplate = await _mongoDbService.FindOne(filter);

            return emailTemplate;
        }

        #endregion
    }
}