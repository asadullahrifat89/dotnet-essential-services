using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using BaseCore.Services;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Repositories;
using IdentityCore.Models.Entities;
using MongoDB.Driver;

namespace IdentityCore.Implementations.Repositories
{
    public class AccountActivationRequestRepository : IAccountActivationRequest

    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public AccountActivationRequestRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContext, IUserRepository userRepository)
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

            var accountActivationRequest = AccountActivationRequest.Initialize(command, authCtx);

            var filter = Builders<AccountActivationRequest>.Filter.Eq(x => x.Email, command.Email);

            var alreadyExistsAccountActivationRequest = await _mongoDbService.FindOne<AccountActivationRequest>(filter);

            if (alreadyExistsAccountActivationRequest != null)
            {

                var newAccountActivationRequest = AccountActivationRequest.Initialize(command, authCtx);

                var update = Builders<AccountActivationRequest>.Update
                    .Set(x => x.ActivationKey, newAccountActivationRequest.ActivationKey);

                await _mongoDbService.UpdateDocument(update: update, filter: filter);

                return Response.BuildServiceResponse().BuildSuccessResponse(newAccountActivationRequest, authCtx?.RequestUri);
            }
            else
            {
                await _mongoDbService.InsertDocument(accountActivationRequest);

                return Response.BuildServiceResponse().BuildSuccessResponse(accountActivationRequest, authCtx?.RequestUri);
            }


        }

        public async Task<ServiceResponse> VerifyAccountActivationRequest(VerifyUserAccountActivationRequestCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<AccountActivationRequest>.Filter.Eq(x => x.Email, command.Email);

            var accountActivationRequest = await _mongoDbService.FindOne<AccountActivationRequest>(filter);

            if (accountActivationRequest != null)
            {
                if (accountActivationRequest.ActivationKey == command.ActivationKey)
                {
                    if(accountActivationRequest.ActivationKeyStatus != ActivationKeyStatus.Expired)
                    {
                        var user = await _userRepository.GetUserByEmail(command.Email);

                        var userFilter = Builders<User>.Filter.Eq(x => x.Email, command.Email);

                        if (user != null)
                        {
                            var updateStatus = Builders<User>.Update
                                .Set(x => x.UserStatus, UserStatus.Active);

                            await _mongoDbService.UpdateDocument(update: updateStatus, filter: userFilter);

                            // set activation key status to expired

                            var updateActivationKeyStatus = Builders<AccountActivationRequest>.Update
                                .Set(x => x.ActivationKeyStatus, ActivationKeyStatus.Expired);

                            await _mongoDbService.UpdateDocument(update: updateActivationKeyStatus, filter: filter);

                            // update password using updatePassword method

                            var updatePassword = new UpdateUserPasswordCommand
                            {
                                UserId = user.Id,
                                NewPassword = command.Password
                            };

                            return await _userRepository.UpdateUserPassword(updatePassword);

                        }
                        else
                        {
                            return Response.BuildServiceResponse().BuildErrorResponse("User not found");
                        }
                        
                    }
                    else
                    {
                        return Response.BuildServiceResponse().BuildErrorResponse("Activation Key expired");
                    };
                }
                else
                {
                    return Response.BuildServiceResponse().BuildErrorResponse("Invalid activation code");
                }
            }
            else
            {
                return Response.BuildServiceResponse().BuildErrorResponse("Account activation request not found");
            }

            #endregion
        }
    }
}
