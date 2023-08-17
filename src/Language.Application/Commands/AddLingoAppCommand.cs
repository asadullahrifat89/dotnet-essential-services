using Base.Application.DTOs.Responses;
using Identity.Application.Extensions;
using Identity.Domain.Entities;
using Language.Domain.Entities;
using MediatR;

namespace Language.Application.Commands
{
    public class AddLingoAppCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public static LanguageApp Initialize(AddLingoAppCommand command, AuthenticationContext authenticationContext)
        {
            var lingoApp = new LanguageApp()
            {
                Name = command.Name,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return lingoApp;
        }
    }
}
