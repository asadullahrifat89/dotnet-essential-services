using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;

namespace IdentityCore.Contracts.Declarations.Repositories
{
    public interface IUserRepository
    {
        Task<ServiceResponse> CreateUser(CreateUserCommand command);

        Task<bool> BeAnExistingUserEmail(string userEmail);

        Task<bool> BeAnExistingPhoneNumber(string phoneNumber);

        Task<bool> BeValidUser(string userEmail, string password);

        Task<User> GetUser(string userEmail, string password);

        Task<User> GetUser(string userId);

        Task<QueryRecordResponse<UserResponse>> GetUser(GetUserQuery query);

        Task<QueryRecordsResponse<UserResponse>> GetUsers(GetUsersQuery query);

        Task<bool> BeAnExistingUser(string userId);

        Task<string[]> GetEndpointList();
    }
}
