using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands;
using IdentityModule.Application.DTOs;
using IdentityModule.Application.Queries;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ServiceResponse> CreateUser(CreateUserCommand command);

        Task<ServiceResponse> UpdateUser(UpdateUserCommand command);

        Task<ServiceResponse> UpdateUserRoles(UpdateUserRolesCommand command);

        Task<bool> BeAnExistingUserEmail(string userEmail);

        Task<bool> BeAnExistingPhoneNumber(string phoneNumber);

        Task<bool> BeValidUser(string userEmail, string password);

        Task<bool> BeValidUserPassword(string userId, string password);

        Task<User> GetUser(string userEmail, string password);

        Task<User> GetUser(string userId);

        Task<QueryRecordResponse<UserResponse>> GetUser(GetUserQuery query);

        Task<QueryRecordsResponse<UserResponse>> GetUsers(GetUsersQuery query);

        Task<bool> BeAnExistingUser(string userId);

        Task<User> GetUserByEmail(string userEmail);

        Task<ServiceResponse> UpdateUserPassword(UpdateUserPasswordCommand command);

        Task<bool> ActivateUser(string id);

        Task<ServiceResponse> UpdateUserPasswordById(string userId, string password);

        Task<ServiceResponse> SubmitUser(SubmitUserCommand request);
    }
}
