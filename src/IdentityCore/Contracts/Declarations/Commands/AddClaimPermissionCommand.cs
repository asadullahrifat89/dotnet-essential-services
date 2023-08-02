using IdentityCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Declarations.Commands
{
    public class AddClaimPermissionCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string RequestUri { get; set; } = string.Empty;
    }
}
