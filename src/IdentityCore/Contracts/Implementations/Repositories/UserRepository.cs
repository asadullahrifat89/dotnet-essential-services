using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Extensions;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MongoDB.Driver;

namespace IdentityCore.Contracts.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IRoleRepository _roleRepository;

        #endregion

        #region Ctor

        public UserRepository(IMongoDbService mongoDbService, IRoleRepository roleRepository)
        {
            _mongoDbService = mongoDbService;
            _roleRepository = roleRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> CreateUser(CreateUserCommand command)
        {
            var user = User.Initialize(command);

            var roles = await _roleRepository.GetRolesByNames(command.Roles);

            var userRoleMaps = new List<UserRoleMap>();

            foreach (var role in roles)
            {
                var roleMap = new UserRoleMap() { 
                
                    UserId = user.Id,
                    RoleId = role.Id,
                };

                userRoleMaps.Add(roleMap);
            }

            await _mongoDbService.InsertDocument(user);
            await _mongoDbService.InsertDocuments(userRoleMaps);

            return Response.BuildServiceResponse().BuildSuccessResponse(user);
        }

        public async Task<bool> BeAnExistingUserEmail(string userEmail)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Email, userEmail);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<bool> BeAnExistingPhoneNumber(string phoneNumber)
        {
            var filter = Builders<User>.Filter.Eq(x => x.PhoneNumber, phoneNumber);

            return await _mongoDbService.Exists(filter);
        }


        public async Task<bool> BeValidUser(string userEmail, string password)
        {
            var encryptedPassword = password.Encrypt();

            var filter = Builders<User>.Filter.And(
                    Builders<User>.Filter.Eq(x => x.Email, userEmail),
                    Builders<User>.Filter.Eq(x => x.Password, encryptedPassword));

            return await _mongoDbService.Exists(filter);
        }

        public async Task<User> GetUser(string userEmail, string password)
        {
            var encryptedPassword = password.Encrypt();

            var filter = Builders<User>.Filter.And(
                   Builders<User>.Filter.Eq(x => x.Email, userEmail),
                   Builders<User>.Filter.Eq(x => x.Password, encryptedPassword));

            return await _mongoDbService.FindOne(filter);
        }

        public async Task<User> GetUser(string userId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);

            return await _mongoDbService.FindOne(filter);
        }

        #endregion
    }
}
