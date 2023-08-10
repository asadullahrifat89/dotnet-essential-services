using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using MediatR;


namespace IdentityCore.Declarations.Commands
{
    public class SendUserAccountActivationRequestCommand : IRequest<ServiceResponse>
    {
        public string Email = string.Empty;
    }
    
}
