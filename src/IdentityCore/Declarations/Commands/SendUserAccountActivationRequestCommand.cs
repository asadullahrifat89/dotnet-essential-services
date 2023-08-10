using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Declarations.Commands
{
    public class SendUserAccountActivationRequestCommand : IRequest<ServiceResponse>
    {
        public string Email = string.Empty;
    }
    
}
