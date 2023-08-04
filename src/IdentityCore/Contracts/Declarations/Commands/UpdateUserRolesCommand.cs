using Amazon.Runtime.Internal;
using IdentityCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IdentityCore.Contracts.Declarations.Commands
{
    public class UpdateUserRolesCommand : IRequest<ServiceResponse> 
    {
        public string UserId { get; set; } = string.Empty;

        public string[] RoleNames { get; set; } = Array.Empty<string>();
    }
}
