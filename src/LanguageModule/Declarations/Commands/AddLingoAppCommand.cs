using BaseModule.Application.DTOs.Responses;
using MediatR;

namespace LanguageModule.Declarations.Commands
{
    public class AddLingoAppCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;
    }
}
