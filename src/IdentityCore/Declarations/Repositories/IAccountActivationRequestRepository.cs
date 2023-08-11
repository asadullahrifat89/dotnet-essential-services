using BaseCore.Models.Responses;
using IdentityCore.Declarations.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Declarations.Repositories
{
    public interface IAccountActivationRequestRepository
    {
        Task<bool> BeAnExistingActivationKey(string activationKey);

        Task<ServiceResponse> CreateAccountActivationRequest(SendUserAccountActivationRequestCommand command);

        Task<ServiceResponse> VerifyAccountActivationRequest(VerifyUserAccountActivationRequestCommand command);
    }
}
