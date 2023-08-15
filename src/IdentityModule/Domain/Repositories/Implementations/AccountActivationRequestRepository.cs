using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using IdentityModule.Application.Commands;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Repositories.Interfaces;
using IdentityModule.Infrastructure.Services.Interfaces;
using MongoDB.Driver;

namespace IdentityModule.Domain.Repositories.Implementations
{
    public class AccountActivationRequestRepository : IAccountActivationRequestRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProviderService _authenticationContext;

        #endregion

        #region Ctor

        public AccountActivationRequestRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProviderService authenticationContext, IUserRepository userRepository)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
            _userRepository = userRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> CreateAccountActivationRequest(SendUserAccountActivationRequestCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var accountActivationRequest = AccountActivationRequest.Initialize(command);

            var filter = Builders<AccountActivationRequest>.Filter.And(
                Builders<AccountActivationRequest>.Filter.Eq(x => x.Email, command.Email),
                Builders<AccountActivationRequest>.Filter.Eq(x => x.ActivationKeyStatus, ActivationKeyStatus.Active));

            var alreadyExistsAccountActivationRequest = await _mongoDbService.FindOne(filter);

            if (alreadyExistsAccountActivationRequest is not null)
            {
                var update = Builders<AccountActivationRequest>.Update
                    .Set(x => x.ActivationKey, accountActivationRequest.ActivationKey);

                var updatedAccountActivationRequest = await _mongoDbService.UpdateById(update: update, alreadyExistsAccountActivationRequest.Id);

                return Response.BuildServiceResponse().BuildSuccessResponse(updatedAccountActivationRequest, authCtx?.RequestUri);
            }
            else
            {
                await _mongoDbService.InsertDocument(accountActivationRequest);

                return Response.BuildServiceResponse().BuildSuccessResponse(accountActivationRequest, authCtx?.RequestUri);
            }
        }

        public async Task<ServiceResponse> VerifyAccountActivationRequest(VerifyUserAccountActivationRequestCommand command)
        {
            var email = command.Email;

            var activationRequest = await GetActiveAccountActivationRequest(email);

            var user = await _userRepository.GetUserByEmail(email);

            var expired = await ExpireActivationKey(activationRequest.Id);

            if (expired)
            {
                var activated = await _userRepository.ActivateUser(user.Id);

                if (activated)
                {
                    return await _userRepository.UpdateUserPasswordById(user.Id, command.Password);
                }
                else
                {
                    return Response.BuildServiceResponse().BuildErrorResponse("Account activation failed.");
                }
            }
            else
            {
                return Response.BuildServiceResponse().BuildErrorResponse("Account activation failed.");
            }
        }

        private async Task<AccountActivationRequest> GetActiveAccountActivationRequest(string email)
        {
            var filter = Builders<AccountActivationRequest>.Filter.And(Builders<AccountActivationRequest>.Filter.Eq(x => x.Email, email), Builders<AccountActivationRequest>.Filter.Eq(x => x.ActivationKeyStatus, ActivationKeyStatus.Active));
            return await _mongoDbService.FindOne(filter);
        }

        private async Task<bool> ExpireActivationKey(string id)
        {
            var update = Builders<AccountActivationRequest>.Update.Set(x => x.ActivationKeyStatus, ActivationKeyStatus.Expired);
            var updatedAccountActivationRequest = await _mongoDbService.UpdateById(update: update, id);
            return updatedAccountActivationRequest is not null;
        }

        public async Task<bool> BeAnExistingActivationKey(string activationKey)
        {
            var filter = Builders<AccountActivationRequest>.Filter.And(
                Builders<AccountActivationRequest>.Filter.Eq(x => x.ActivationKey, activationKey),
                Builders<AccountActivationRequest>.Filter.Eq(x => x.ActivationKeyStatus, ActivationKeyStatus.Active));

            return await _mongoDbService.Exists(filter);
        }

        #endregion
    }
}
