using BaseCore.Extensions;
using FluentValidation;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;

namespace TeamsCore.Implementations.Queries.Validators
{
    public class GetSearchCriteriaQueryValidator : AbstractValidator<GetSearchCriteriaQuery>
    {
        private readonly ISearchCriteriaRepository _searchCriteriaRepository;

        public GetSearchCriteriaQueryValidator(ISearchCriteriaRepository searchCriteriaRepository)
        {
            _searchCriteriaRepository = searchCriteriaRepository;

            RuleFor(x => x.SearchCriteriaId).NotNull()
                                            .NotEmpty()
                                            .WithMessage("SearchCriteria Id must not be empty");

            RuleFor(x => x).MustAsync(BeAnExistingSearchCriteriaById).WithMessage("SearchCriteria does not exist.").When(x => !x.SearchCriteriaId.IsNullOrBlank());

           // RuleFor(x => x.SearchCriteriaName).MustAsync(BeAnExistingSearchCriteria).When(x => !x.SearchCriteriaName.IsNullOrBlank()).WithMessage("SearchCriteria does not exist.");
        }

        /*private async Task<bool> BeAnExistingSearchCriteria(string SearchCriteriaName, CancellationToken token)
        {
            return await _searchCriteriaRepository.BeAnExistingSearchCriteria(searchCriteria: SearchCriteriaName);
        }*/

        private async Task<bool> BeAnExistingSearchCriteriaById(GetSearchCriteriaQuery query, CancellationToken token)
        {
            return await _searchCriteriaRepository.BeAnExistingSearchCriteriaById(searchCriteriaId: query.SearchCriteriaId);
        }
    }
}
