using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;
using MediatR;


namespace IdentityModule.Application.Commands
{
    public class SendUserAccountActivationRequestCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public static AccountActivationRequest Initialize(SendUserAccountActivationRequestCommand command)
        {
            return new AccountActivationRequest()
            {
                Email = command.Email,
                ActivationKey = AccountActivationRequest.GenerateRandomNumber().ToString("000000"),
            };
        }
    }
}
