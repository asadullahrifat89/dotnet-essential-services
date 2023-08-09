using BaseCore.Extensions;
using FluentValidation;
using LingoCore.Declarations.Commands;
using LingoCore.Declarations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingoCore.Implementations.Commands.Validators
{
    public class AddLingoResourcesCommandValidator : AbstractValidator<AddLingoResourcesCommand>
    {
        private readonly ILingoAppRepository _lingoAppRepository;

        public AddLingoResourcesCommandValidator(ILingoAppRepository lingoAppRepository)
        {
            _lingoAppRepository = lingoAppRepository;
            

            RuleFor(x => x.AppId).NotNull().NotEmpty().WithMessage("App ID is required");
            
            RuleFor(x => x.AppId).MustAsync(BeAnExistingLingoApp).WithMessage("Lingo app does not exist").When(x=>!x.AppId.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingLingoApp(string appId, CancellationToken token)
        {
            return await _lingoAppRepository.BeAnExistingLingoAppById(appId);
        }
    }
}
