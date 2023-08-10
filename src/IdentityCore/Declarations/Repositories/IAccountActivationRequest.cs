using BaseCore.Models.Responses;
using IdentityCore.Declarations.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Declarations.Repositories
{
    public interface IAccountActivationRequest
    {
        Task<ServiceResponse> CreateAccountActivationRequest(SendUserAccountActivationRequestCommand command);
    }
}
