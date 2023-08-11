using BaseCore.Models.Responses;
using MediatR;

namespace LingoCore.Declarations.Queries
{
    public class GetLingoResourcesInFormatQuery : IRequest<QueryRecordResponse<Dictionary<string, string>>>
    {
        public string AppId { get; set; } = string.Empty;

        public string Format { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;
    }
}
