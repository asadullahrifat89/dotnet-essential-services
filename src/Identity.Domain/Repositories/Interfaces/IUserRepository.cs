using Identity.Domain.Entities;

namespace Identity.Domain.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user, string[] roles);

        Task<User> UpdateUser(User user);

        Task<UserRoleMap[]> UpdateUserRoles(string userId, string[] roleNames);

        Task<bool> BeAnExistingUserEmail(string userEmail);

        Task<bool> BeAnExistingPhoneNumber(string phoneNumber);

        Task<bool> BeValidUser(string userEmail, string password);

        Task<bool> BeValidUserPassword(string userId, string password);

        Task<User> GetUser(string userEmail, string password);

        Task<User> GetUserById(string userId);

        Task<User> GetUser(string userId);

        Task<User[]> GetUsers(string searchTerm, int pageIndex, int pageSize);

        Task<bool> BeAnExistingUser(string userId);

        Task<User> GetUserByEmail(string userEmail);

        Task<User> UpdateUserPassword(string userId, string oldPassword, string newPassword);

        Task<bool> ActivateUser(string id);

        Task<User> UpdateUserPasswordById(string userId, string password);

        Task<User> SubmitUser(User user);
    }
}
