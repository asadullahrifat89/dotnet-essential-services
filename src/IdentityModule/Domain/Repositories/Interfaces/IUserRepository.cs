using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.DTOs;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ServiceResponse> CreateUser(User user, string[] roles);

        Task<ServiceResponse> UpdateUser(User user);

        Task<ServiceResponse> UpdateUserRoles(string userId, string[] roleNames);

        Task<bool> BeAnExistingUserEmail(string userEmail);

        Task<bool> BeAnExistingPhoneNumber(string phoneNumber);

        Task<bool> BeValidUser(string userEmail, string password);

        Task<bool> BeValidUserPassword(string userId, string password);

        Task<User> GetUser(string userEmail, string password);

        Task<User> GetUserById(string userId);

        Task<QueryRecordResponse<UserResponse>> GetUser(string userId);

        Task<QueryRecordsResponse<UserResponse>> GetUsers(string searchTerm, int pageIndex, int pageSize);

        Task<bool> BeAnExistingUser(string userId);

        Task<User> GetUserByEmail(string userEmail);

        Task<ServiceResponse> UpdateUserPassword(string userId, string oldPassword, string newPassword);

        Task<bool> ActivateUser(string id);

        Task<ServiceResponse> UpdateUserPasswordById(string userId, string password);

        Task<ServiceResponse> SubmitUser(User user);
    }
}
