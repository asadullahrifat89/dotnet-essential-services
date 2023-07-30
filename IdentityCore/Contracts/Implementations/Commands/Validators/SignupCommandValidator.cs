using FluentValidation;
using IdentityCore.Contracts.Declarations.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Implementations.Commands.Validators
{
    public class SignupCommandValidator : AbstractValidator<SignupCommand>
    {
        public SignupCommandValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();

            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.Lastname).NotNull().NotEmpty();

            RuleFor(x => x.Phone).NotNull();
        }
    }
}
