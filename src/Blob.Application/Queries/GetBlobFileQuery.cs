using Base.Application.DTOs.Responses;
using Blob.Domain.Entities;
using MediatR;

namespace Blob.Application.Queries
{
    public class GetBlobFileQuery : IRequest<QueryRecordResponse<BlobFile>>
    {
        public string FileId { get; set; } = string.Empty;
    }
}
