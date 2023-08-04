using FluentValidation;
using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
