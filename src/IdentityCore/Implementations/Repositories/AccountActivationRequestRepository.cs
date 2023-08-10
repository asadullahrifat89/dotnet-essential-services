using BaseCore.Models.Responses;
using BaseCore.Services;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;
using IdentityCore.Models.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Implementations.Repositories
{
    public class AccountActivationRequestRepository : IAccountActivationRequest
  
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public AccountActivationRequestRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> CreateAccountActivationRequest(SendUserAccountActivationRequestCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var accountActivationRequest = AccountActivationRequest.Initialize(command, authCtx);
            
            var filter = Builders<AccountActivationRequest>.Filter.Eq(x => x.Email, command.Email);

            var alreadyExistsAccountActivationRequest = await _mongoDbService.FindOne<AccountActivationRequest>(filter);

            if (alreadyExistsAccountActivationRequest != null)
            {
        
                var newAccountActivationRequest = AccountActivationRequest.Initialize(command, authCtx);

                var update = Builders<AccountActivationRequest>.Update
                    .Set(x => x.ActivationKey, newAccountActivationRequest.ActivationKey);

                await _mongoDbService.UpdateDocument(update: update,  filter: filter);

                return Response.BuildServiceResponse().BuildSuccessResponse(newAccountActivationRequest, authCtx?.RequestUri);
            }
            else
            {
                await _mongoDbService.InsertDocument(accountActivationRequest);

                return Response.BuildServiceResponse().BuildSuccessResponse(accountActivationRequest, authCtx?.RequestUri);
            }

            
        }


       

        #endregion
    }

    
}
