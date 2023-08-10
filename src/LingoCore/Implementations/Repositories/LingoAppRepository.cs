﻿using BaseCore.Models.Responses;
using BaseCore.Services;
using LingoCore.Declarations.Commands;
using LingoCore.Declarations.Queries;
using LingoCore.Declarations.Repositories;
using LingoCore.Models.Entities;
using MongoDB.Driver;

namespace LingoCore.Implementations.Repositories
{
    public class LingoAppRepository : ILingoAppRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public LingoAppRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContextProvider)
        {
            _mongoDbService = mongoDbService;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> AddLingoApp(AddLingoAppCommand command)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var lingoApp = LingoApp.Initialize(command, authCtx);

            await _mongoDbService.InsertDocument(lingoApp);

            return Response.BuildServiceResponse().BuildSuccessResponse(lingoApp, authCtx?.RequestUri);
        }

        public async Task<QueryRecordResponse<LingoApp>> GetLingoApp(GetLingoAppQuery query)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<LingoApp>.Filter.Where(x => x.Id.Equals(query.AppId));

            var user = await _mongoDbService.FindOne(filter);

            return Response.BuildQueryRecordResponse<LingoApp>().BuildSuccessResponse(user, authCtx?.RequestUri);

        }

        public Task<bool> BeAnExistingLingoApp(string appName)
        {
            var filter = Builders<LingoApp>.Filter.Where(x => x.Name.ToLower().Equals(appName.ToLower()));

            return _mongoDbService.Exists<LingoApp>(filter);
        }

        public Task<bool> BeAnExistingLingoAppById(string appId)
        {
            var filter = Builders<LingoApp>.Filter.Where(x => x.Id.Equals(appId));

            return _mongoDbService.Exists<LingoApp>(filter);
        }

        #endregion
    }
}