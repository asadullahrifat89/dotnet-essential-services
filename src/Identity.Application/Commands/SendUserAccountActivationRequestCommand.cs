using Base.Application.DTOs.Responses;
using Identity.Domain.Entities;
using MediatR;


namespace Identity.Application.Commands
{
    public class SendUserAccountActivationRequestCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public static AccountActivationRequest Map(SendUserAccountActivationRequestCommand command)
        {
            return new AccountActivationRequest()
            {
                Email = command.Email,
                ActivationKey = AccountActivationRequest.GenerateRandomNumber().ToString("000000"),
            };
        }
    }
}
