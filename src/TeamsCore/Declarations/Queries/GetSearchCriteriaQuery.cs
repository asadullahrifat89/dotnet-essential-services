using BaseCore.Models.Responses;
using MediatR;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Queries
{
    public class GetSearchCriteriaQuery : IRequest<QueryRecordResponse<SearchCriteria>>
    {
        public string SearchCriteriaId { get; set; } = string.Empty;

       // public string? SearchCriteriaName { get; set; } = string.Empty;
    }
}
