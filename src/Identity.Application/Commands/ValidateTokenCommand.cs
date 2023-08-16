using Base.Application.DTOs.Responses;
using MediatR;

namespace Identity.Application.Commands
{
    public class ValidateTokenCommand : IRequest<ServiceResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
