using Base.Infrastructure.Providers.Interfaces;
using Identity.Application.Providers.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Repositories.Interfaces;
using MongoDB.Driver;

namespace Identity.Infrastructure.Persistence
{
    public class AccountActivationRequestRepository : IAccountActivationRequestRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public AccountActivationRequestRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContext, IUserRepository userRepository)
        {
            _mongoDbContextProvider = mongoDbService;
            _authenticationContextProvider = authenticationContext;
            _userRepository = userRepository;
        }

        #endregion

        #region Methods

        public async Task<AccountActivationRequest> CreateAccountActivationRequest(AccountActivationRequest accountActivationRequest)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<AccountActivationRequest>.Filter.And(
                Builders<AccountActivationRequest>.Filter.Eq(x => x.Email, accountActivationRequest.Email),
                Builders<AccountActivationRequest>.Filter.Eq(x => x.ActivationKeyStatus, ActivationKeyStatus.Active));

            var alreadyExistsAccountActivationRequest = await _mongoDbContextProvider.FindOne(filter);

            if (alreadyExistsAccountActivationRequest is not null)
            {
                var update = Builders<AccountActivationRequest>.Update
                    .Set(x => x.ActivationKey, accountActivationRequest.ActivationKey);

                var updatedAccountActivationRequest = await _mongoDbContextProvider.UpdateById(update: update, alreadyExistsAccountActivationRequest.Id);

                return updatedAccountActivationRequest;
                //return Response.BuildServiceResponse().BuildSuccessResponse(updatedAccountActivationRequest, authCtx?.RequestUri);
            }
            else
            {
                await _mongoDbContextProvider.InsertDocument(accountActivationRequest);

                return accountActivationRequest;
                //return Response.BuildServiceResponse().BuildSuccessResponse(accountActivationRequest, authCtx?.RequestUri);
            }
        }

        public async Task<User> VerifyAccountActivationRequest(string email, string activationKey, string password)
        {
            var activationRequest = await GetActiveAccountActivationRequestForEmailAndKey(email: email, activationKey: activationKey);

            var user = await _userRepository.GetUserByEmail(email);

            var expired = await ExpireActivationKey(activationRequest.Id);

            if (expired)
            {
                var activated = await _userRepository.ActivateUser(user.Id);

                if (activated)
                {
                    return await _userRepository.UpdateUserPasswordById(user.Id, password);
                }
                //else
                //{
                //    return Response.BuildServiceResponse().BuildErrorResponse("Account activation failed.");
                //}
            }

            return default;
            //else
            //{
            //    return Response.BuildServiceResponse().BuildErrorResponse("Account activation failed.");
            //}
        }

        private async Task<AccountActivationRequest> GetActiveAccountActivationRequestForEmailAndKey(string email, string activationKey)
        {
            var filter = Builders<AccountActivationRequest>.Filter.And(
                Builders<AccountActivationRequest>.Filter.Eq(x => x.Email, email),
                Builders<AccountActivationRequest>.Filter.Eq(x => x.ActivationKey, activationKey),
                Builders<AccountActivationRequest>.Filter.Eq(x => x.ActivationKeyStatus, ActivationKeyStatus.Active));

            return await _mongoDbContextProvider.FindOne(filter);
        }

        private async Task<bool> ExpireActivationKey(string id)
        {
            var update = Builders<AccountActivationRequest>.Update.Set(x => x.ActivationKeyStatus, ActivationKeyStatus.Expired);
            var updatedAccountActivationRequest = await _mongoDbContextProvider.UpdateById(update: update, id);
            return updatedAccountActivationRequest is not null;
        }

        public async Task<bool> BeAnExistingActivationKey(string activationKey)
        {
            var filter = Builders<AccountActivationRequest>.Filter.And(
                Builders<AccountActivationRequest>.Filter.Eq(x => x.ActivationKey, activationKey),
                Builders<AccountActivationRequest>.Filter.Eq(x => x.ActivationKeyStatus, ActivationKeyStatus.Active));

            return await _mongoDbContextProvider.Exists(filter);
        }

        #endregion
    }
}
