using FluentValidation;
using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Implementations.Queries.Validators
{
    public class GetEndPointsQueryValidator : AbstractValidator<GetEndPointsQuery>
    {
        private readonly IUserRepository _userRepository;

        public GetEndPointsQueryValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            //RuleFor(x => x.UserId).NotNull().NotEmpty();

        }

    }
}
