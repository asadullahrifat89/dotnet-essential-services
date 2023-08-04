using BaseCore.Models.Responses;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Queries;
using IdentityCore.Models.Entities;

namespace IdentityCore.Declarations.Repositories
{
    public interface IUserRepository
    {
        Task<ServiceResponse> CreateUser(CreateUserCommand command);

        Task<ServiceResponse> UpdateUser(UpdateUserCommand command);

        Task<ServiceResponse> UpdateUserRoles(UpdateUserRolesCommand command);

        Task<bool> BeAnExistingUserEmail(string userEmail);

        Task<bool> BeAnExistingPhoneNumber(string phoneNumber);

        Task<bool> BeValidUser(string userEmail, string password);

        Task<User> GetUser(string userEmail, string password);

        Task<User> GetUser(string userId);

        Task<QueryRecordResponse<UserResponse>> GetUser(GetUserQuery query);

        Task<QueryRecordsResponse<UserResponse>> GetUsers(GetUsersQuery query);

        Task<bool> BeAnExistingUser(string userId);

    }
}
