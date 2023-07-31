using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Declarations.Repositories
{
    public interface IUserRepository
    {
        Task<ServiceResponse> Signup(CreateUserCommand command);

        Task<bool> BeAnExistingUserEmail(string userEmail);

        Task<User> GetUser(string userEmail, string password);
    }
}
