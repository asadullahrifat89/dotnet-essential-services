using BaseCore.Models.Responses;
using BlobCore.Models.Entities;
using MediatR;

namespace BlobCore.Declarations.Queries
{
    public class GetBlobFileQuery: IRequest<QueryRecordResponse<BlobFile>>
    {
        public string FileId { get; set; } = string.Empty;
    }
}
