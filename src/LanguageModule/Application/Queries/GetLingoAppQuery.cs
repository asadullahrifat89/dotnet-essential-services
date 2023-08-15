using BaseModule.Application.DTOs.Responses;
using LanguageModule.Domain.Entities;
using MediatR;

namespace LanguageModule.Application.Quaries
{
    public class GetLingoAppQuery : IRequest<QueryRecordResponse<LingoApp>>
    {
        public string AppId { get; set; } = string.Empty;
    }
}
