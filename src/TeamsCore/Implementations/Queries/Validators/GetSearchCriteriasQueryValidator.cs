using FluentValidation;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Models.Entities;

namespace TeamsCore.Implementations.Queries.Validators
{
    public class GetSearchCriteriasQueryValidator : AbstractValidator<GetSearchCriteriasQuery>
    {
        private readonly ISearchCriteriaRepository _searchCriteriaRepository;
        
        public GetSearchCriteriasQueryValidator(ISearchCriteriaRepository searchCriteriaRepository)
        {
            _searchCriteriaRepository = searchCriteriaRepository;

            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);

            RuleFor(x => x.PageSize).GreaterThan(0);


            RuleFor(x => x.SearchCriteriaType)
                .Must(BeAnExistingSearchCriteriaType)
                .When(x => x.SearchCriteriaType.HasValue)
                .WithMessage("Invalid SearchCriteriaType");

            RuleFor(x => x.SkillsetType)
                .Must(BeAnExistingSkillsetType)
                .When(x => x.SkillsetType.HasValue)
                .WithMessage("Invalid SkillsetType");

        }

        private bool BeAnExistingSearchCriteriaType(SearchCriteriaType? searchCriteriaType)
        {
            return Enum.IsDefined(typeof(SearchCriteriaType), searchCriteriaType);
        }

        private bool BeAnExistingSkillsetType(SkillsetType? skillsetType)
        {
            return Enum.IsDefined(typeof(SkillsetType), skillsetType);
        }
    }
}
