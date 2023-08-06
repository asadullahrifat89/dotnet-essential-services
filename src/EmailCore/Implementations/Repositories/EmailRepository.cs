using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using BaseCore.Services;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Queries;
using EmailCore.Declarations.Repositories;
using EmailCore.Models.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<ServiceResponse> CreateTemplate(CreateTemplateCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task<QueryRecordResponse<EmailTemplate>> GetEmailTemplate(GetEmailTemplateQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Id, query.TemplateId);

            var emailTemplate = await _mongoDbService.FindOne(filter);

            return emailTemplate == null ?Response.BuildQueryRecordResponse<EmailTemplate>().BuildErrorResponse(new ErrorResponse().BuildExternalError("Template doesn't exist."), authCtx?.RequestUri) : Response.BuildQueryRecordResponse<EmailTemplate>().BuildSuccessResponse(emailTemplate, authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingEmailTemplate(string templateId)
        {
            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Id, templateId);

            return await _mongoDbService.Exists<EmailTemplate>(filter);
        }
        #endregion

    }
}
