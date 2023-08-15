using BaseModule.Application.DTOs.Responses;
using LanguageModule.Domain.Entities;
using MediatR;

namespace LanguageModule.Application.Queries
{
    public class GetLingoAppQuery : IRequest<QueryRecordResponse<LanguageApp>>
    {
        public string AppId { get; set; } = string.Empty;
    }
}
