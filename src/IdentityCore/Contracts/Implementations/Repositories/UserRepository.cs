using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Contracts.Implementations.Services;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;

        #endregion

        #region Ctor

        public UserRepository(IMongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Signup(CreateUserCommand command)
        {
            var user = User.Initialize(command);

            await _mongoDbService.InsertDocument(user);

            return Response.Build().BuildSuccessResponse(user);
        }

        public async Task<bool> BeAnExistingUserEmail(string userEmail)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Email, userEmail);

            return await _mongoDbService.Exists(filter);
        }

        #endregion
    }
}
