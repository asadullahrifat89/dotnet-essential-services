using IdentityCore.Contracts.Declarations.Commands;
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
    }
}
