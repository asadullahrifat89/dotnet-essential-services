using FluentValidation;

namespace Email.Application.Queries.Validators
{
    public class GetEmailTemplateByPurposeQueryValidator : AbstractValidator<GetEmailTemplateByPurposeQuery>
    {  
        public GetEmailTemplateByPurposeQueryValidator()
        {
            RuleFor(x => x.Purpose).NotNull().NotEmpty();            
        }
    }
}
