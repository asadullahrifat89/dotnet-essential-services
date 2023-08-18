using Base.Application.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.ContentMangement.Application.Queries;
using Teams.ContentMangement.Domain.Repositories.Interfaces;
using Teams.CustomerEngagement.Domain.Repositories.Interfaces;

namespace Teams.CustomerEngagement.Application.Queries.Validators
{
    public class GetQuotationQueryValidator : AbstractValidator<GetQuotationQuery>
    {
        private readonly IQuotationRepository _QuotationRepository;

        public GetQuotationQueryValidator(IQuotationRepository QuotationRepository)
        {
            _QuotationRepository = QuotationRepository;

            RuleFor(x => x.QuotationId).NotNull().NotEmpty();
            RuleFor(x => x.QuotationId).MustAsync(BeAnExistingQuotation).WithMessage("Quotation doesn't exist.").When(x => !x.QuotationId.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingQuotation(string QuotationId, CancellationToken arg2)
        {
            return await _QuotationRepository.BeAnExistingQuotationId(QuotationId);
        }
    }
}
