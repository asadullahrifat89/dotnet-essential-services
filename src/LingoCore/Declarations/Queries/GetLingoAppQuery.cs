using BaseCore.Models.Responses;
using LingoCore.Models.Entities;
using MediatR;

namespace LingoCore.Declarations.Queries
{
    public class GetLingoAppQuery: IRequest<QueryRecordResponse<LingoApp>>
    {
        public string AppId { get; set; } = string.Empty;
    }
}
