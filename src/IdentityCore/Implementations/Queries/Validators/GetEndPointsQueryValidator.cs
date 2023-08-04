using FluentValidation;
using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;

namespace IdentityCore.Implementations.Queries.Validators
{
    public class GetEndPointsQueryValidator : AbstractValidator<GetEndPointsQuery>
    {
        private readonly IEndpointsRepository _endpointsRepository;

        public GetEndPointsQueryValidator(IEndpointsRepository endpointsRepository)
        {
            _endpointsRepository = endpointsRepository;
        }
    }
}
