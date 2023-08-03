using FluentValidation;
using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Implementations.Repositories;
using IdentityCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Implementations.Queries.Validators
{
    public class GetRoleQueryValidator : AbstractValidator<GetRoleQuery>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;



        public GetRoleQueryValidator(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.UserId).MustAsync(BeAnExistingUser).WithMessage("User doesn't exist.").When(x => !x.UserId.IsNullOrBlank());
        }



        private async Task<bool> BeAnExistingUser(string userId, CancellationToken arg2)
        {
            return await _userRepository.BeAnExistingUser(userId);
        }
    }
}
