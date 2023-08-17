using Base.Application.DTOs.Responses;
using Language.Domain.Entities;
using MediatR;

namespace Language.Application.Queries
{
    public class GetLingoAppQuery : IRequest<QueryRecordResponse<LanguageApp>>
    {
        public string AppId { get; set; } = string.Empty;
    }
}
