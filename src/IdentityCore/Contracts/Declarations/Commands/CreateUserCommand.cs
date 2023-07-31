using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Declarations.Commands
{
    public class CreateUserCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string[] Roles { get; set; } = new string[0];
    }
}
