using BaseCore.Models.Responses;
using MediatR;

namespace LingoCore.Declarations.Commands
{
    public class AddLingoAppCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;
    }
}
