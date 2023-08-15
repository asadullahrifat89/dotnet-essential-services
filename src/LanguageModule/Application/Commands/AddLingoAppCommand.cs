using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;
using IdentityModule.Infrastructure.Extensions;
using LanguageModule.Domain.Entities;
using MediatR;

namespace LanguageModule.Application.Commands
{
    public class AddLingoAppCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public static LingoApp Initialize(AddLingoAppCommand command, AuthenticationContext authenticationContext)
        {
            var lingoApp = new LingoApp()
            {
                Name = command.Name,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return lingoApp;
        }
    }
}
