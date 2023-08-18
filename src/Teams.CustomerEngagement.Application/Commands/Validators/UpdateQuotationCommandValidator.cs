using Base.Application.Extensions;
using FluentValidation;
using Teams.CustomerEngagement.Domain.Repositories.Interfaces;

namespace Teams.CustomerEngagement.Application.Commands.Validators
{
    public class UpdateQuotationCommandValidator : AbstractValidator<UpdateQuotationCommand>
    {
        private readonly IQuotationRepository _quotationRepository;

        public UpdateQuotationCommandValidator(IQuotationRepository quotationRepository)
        {
            _quotationRepository = quotationRepository;

            RuleFor(x => x.QuotationId).NotNull().NotEmpty().WithMessage("QuotationId must not be empty");
            RuleFor(x => x.QuotationId)
               .MustAsync(BeAnExistingQuotationId)
               .WithMessage("Product does not exist").When(x => !x.QuotationId.IsNullOrBlank());

            RuleFor(x => x.QuoteStatus).IsInEnum().WithMessage("QuoteStatus invalid.");
            RuleFor(x => x.Priority).IsInEnum().WithMessage("Priority invalid.");
            RuleFor(x => x.AssignedTeamsUsers).NotNull();
            RuleFor(x => x.LinkedQuotationDocuments).NotNull();
        }

        private async Task<bool> BeAnExistingQuotationId(string quotationId, CancellationToken token)
        {
            return await _quotationRepository.BeAnExistingQuotationId(quotationId);
        }
    }
}
