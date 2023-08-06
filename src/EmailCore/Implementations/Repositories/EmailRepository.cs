﻿using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using BaseCore.Services;
using EmailCore.Declarations.Commands;
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

        public async Task<ServiceResponse> UpdateTemplate(UpdateTemplateCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();



            var update = Builders<EmailTemplate>.Update
                .Set(x => x.Name, command.Name)
                .Set(x => x.Body, command.Body)
                .Set(x => x.EmailTemplateType, command.EmailTemplateType)
                .Set(x => x.Tags, command.Tags);

            await _mongoDbService.UpdateById(update: update, id: command.TemplateId);

            var updatedTemplate = await _mongoDbService.FindById<EmailTemplate>(command.TemplateId);

            return Response.BuildServiceResponse().BuildSuccessResponse(updatedTemplate, authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingTemplate(string templateId)
        {
            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Id, templateId);

            return await _mongoDbService.Exists(filter);
        }


        #endregion
    }
}
