using BaseModule.Application.DTOs.Responses;
using BaseModule.Domain.Entities;
using BaseModule.Domain.Repositories.Interfaces;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Infrastructure.Services.Interfaces;
using IdentityModule.Declarations.Commands;
using IdentityModule.Declarations.Queries;
using IdentityModule.Declarations.Repositories;
using IdentityModule.Models.Entities;
using MongoDB.Driver;

namespace IdentityModule.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Fields

        private readonly IMongoDbRepository _mongoDbService;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthenticationContextProviderService _authenticationContext;

        #endregion

        #region Ctor

        public UserRepository(IMongoDbRepository mongoDbService, IRoleRepository roleRepository, IAuthenticationContextProviderService authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _roleRepository = roleRepository;
            _authenticationContext = authenticationContext;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> CreateUser(CreateUserCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var user = User.Initialize(command, authCtx);

            var userRoleMaps = new List<UserRoleMap>();

            // if roles were sent map user to role
            if (command.Roles != null && command.Roles.Any())
            {
                var roles = await _roleRepository.GetRolesByNames(command.Roles);

                foreach (var role in roles)
                {
                    var roleMap = new UserRoleMap()
                    {
                        UserId = user.Id,
                        RoleId = role.Id,
                    };

                    userRoleMaps.Add(roleMap);
                }
            }

            await _mongoDbService.InsertDocument(user);

            if (userRoleMaps.Any())
                await _mongoDbService.InsertDocuments(userRoleMaps);

            return Response.BuildServiceResponse().BuildSuccessResponse(user, authCtx?.RequestUri);
        }

        public async Task<ServiceResponse> UpdateUser(UpdateUserCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var update = Builders<User>.Update
                .Set(x => x.FirstName, command.FirstName)
                .Set(x => x.LastName, command.LastName)
                .Set(x => x.ProfileImageUrl, command.ProfileImageUrl)
                .Set(x => x.Address, command.Address)
                .Set(x => x.TimeStamp.ModifiedOn, DateTime.UtcNow)
                .Set(x => x.TimeStamp.ModifiedBy, authCtx.User?.Id);

            await _mongoDbService.UpdateById(update: update, id: command.UserId);
            var updatedUser = await _mongoDbService.FindById<User>(command.UserId);

            return Response.BuildServiceResponse().BuildSuccessResponse(updatedUser, authCtx?.RequestUri);
        }

        public async Task<ServiceResponse> UpdateUserPassword(UpdateUserPasswordCommand command)
        {
            return await UpdateUserPasswordById(command.UserId, command.NewPassword);
        }

        public async Task<ServiceResponse> UpdateUserPasswordById(string userId, string password)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var update = Builders<User>.Update.Set(x => x.Password, password.Encrypt());

            var updatedUser = await _mongoDbService.UpdateById(update: update, id: userId);

            return Response.BuildServiceResponse().BuildSuccessResponse(updatedUser, authCtx?.RequestUri);
        }

        public async Task<ServiceResponse> UpdateUserRoles(UpdateUserRolesCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var exisitingUserRoleMaps = await _mongoDbService.GetDocuments(Builders<UserRoleMap>.Filter.Eq(x => x.UserId, command.UserId));

            var roles = await _roleRepository.GetRolesByNames(command.RoleNames);

            var newUserRoleMaps = new List<UserRoleMap>();

            foreach (var role in roles)
            {
                var roleMap = new UserRoleMap()
                {
                    UserId = command.UserId,
                    RoleId = role.Id,
                };

                newUserRoleMaps.Add(roleMap);
            }

            if (exisitingUserRoleMaps.Any())
                await _mongoDbService.DeleteDocuments(Builders<UserRoleMap>.Filter.In(x => x.Id, exisitingUserRoleMaps.Select(x => x.Id).ToArray()));

            if (newUserRoleMaps.Any())
                await _mongoDbService.InsertDocuments(newUserRoleMaps);

            return Response.BuildServiceResponse().BuildSuccessResponse(newUserRoleMaps, authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingUserEmail(string userEmail)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Email, userEmail);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<bool> BeValidUserPassword(string userId, string password)
        {
            var filter = Builders<User>.Filter.And(Builders<User>.Filter.Eq(x => x.Id, userId), Builders<User>.Filter.Eq(x => x.Password, password.Encrypt()));

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

        public async Task<User> GetUserByEmail(string userEmail)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Email, userEmail);

            return await _mongoDbService.FindOne(filter);
        }

        public async Task<User> GetUser(string userId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);

            return await _mongoDbService.FindOne(filter);
        }

        public async Task<QueryRecordResponse<UserResponse>> GetUser(GetUserQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<User>.Filter.Eq(x => x.Id, query.UserId);

            var user = await _mongoDbService.FindOne(filter);

            return Response.BuildQueryRecordResponse<UserResponse>().BuildSuccessResponse(UserResponse.Initialize(user), authCtx?.RequestUri);
        }

        public async Task<QueryRecordsResponse<UserResponse>> GetUsers(GetUsersQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<User>.Filter.Empty;

            if (!query.SearchTerm.IsNullOrBlank())
            {
                filter &= Builders<User>.Filter.Or(
                    Builders<User>.Filter.Where(x => x.FirstName.ToLower().Contains(query.SearchTerm.ToLower())),
                    Builders<User>.Filter.Where(x => x.LastName.ToLower().Contains(query.SearchTerm.ToLower())),
                    Builders<User>.Filter.Where(x => x.DisplayName.ToLower().Contains(query.SearchTerm.ToLower())),
                    Builders<User>.Filter.Where(x => x.Email.ToLower().Contains(query.SearchTerm.ToLower())));
            }

            var count = await _mongoDbService.CountDocuments(filter: filter);

            var users = await _mongoDbService.GetDocuments(
                filter: filter,
                skip: query.PageIndex * query.PageSize,
                limit: query.PageSize);

            return new QueryRecordsResponse<UserResponse>().BuildSuccessResponse(
               count: count,
               records: users is not null ? users.Select(x => UserResponse.Initialize(x)).ToArray() : Array.Empty<UserResponse>(), authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingUser(string id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            return await _mongoDbService.Exists(filter);
        }

        public async Task<bool> ActivateUser(string id)
        {
            var updateStatus = Builders<User>.Update.Set(x => x.UserStatus, UserStatus.Active);
            var updatedUser = await _mongoDbService.UpdateById(update: updateStatus, id);

            return updatedUser is not null;
        }

        public async Task<ServiceResponse> SubmitUser(SubmitUserCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var user = User.Initialize(command, authCtx);

            await _mongoDbService.InsertDocument(user);

            return Response.BuildServiceResponse().BuildSuccessResponse(user, authCtx?.RequestUri);
        }

        #endregion
    }
}