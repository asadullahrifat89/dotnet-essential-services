using BaseModule.Domain.DTOs.Responses;
using BlobModule.Models.Entities;
using MediatR;

namespace BlobModule.Declarations.Queries
{
    public class GetBlobFileQuery : IRequest<QueryRecordResponse<BlobFile>>
    {
        public string FileId { get; set; } = string.Empty;
    }
}
