using Amazon.Runtime.Internal;
using BaseCore.Models.Responses;
using MediatR;
using TeamsCore.Declarations.Queries;
using TeamsCore.Models.Entities;

namespace TeamsCore.Implementations.Queries.Handlers
{
    public class GetSearchCriteriaQueryHandler : IRequestHandler<GetSearchCriteriaQuery, QueryRecordsResponse<SearchCriteria>>
    {
        public Task<QueryRecordsResponse<SearchCriteria>> Handle(GetSearchCriteriaQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
