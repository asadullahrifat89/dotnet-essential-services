using BaseModule.Models.Responses;
using LanguageModule.Models.Entities;
using MediatR;

namespace LanguageModule.Declarations.Queries
{
    public class GetLingoAppQuery : IRequest<QueryRecordResponse<LingoApp>>
    {
        public string AppId { get; set; } = string.Empty;
    }
}
