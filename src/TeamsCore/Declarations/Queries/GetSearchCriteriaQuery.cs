using BaseCore.Models.Responses;
using MediatR;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Queries
{
    public class GetSearchCriteriaQuery : IRequest<QueryRecordsResponse<SearchCriteria>>
    {
    }
}
