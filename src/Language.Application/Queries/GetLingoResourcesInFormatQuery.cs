using Base.Application.DTOs.Responses;
using MediatR;

namespace Language.Application.Queries
{
    public class GetLingoResourcesInFormatQuery : IRequest<QueryRecordResponse<Dictionary<string, string>>>
    {
        public string AppId { get; set; } = string.Empty;

        public string Format { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;
    }
}
