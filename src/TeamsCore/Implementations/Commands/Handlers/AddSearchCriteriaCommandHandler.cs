using BaseCore.Models.Responses;
using MediatR;
using TeamsCore.Declarations.Commands;

namespace TeamsCore.Implementations.Commands.Handlers
{
    public class AddSearchCriteriaCommandHandler : IRequestHandler<AddSearchCriteriaCommand, ServiceResponse>
    {
        public Task<ServiceResponse> Handle(AddSearchCriteriaCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
