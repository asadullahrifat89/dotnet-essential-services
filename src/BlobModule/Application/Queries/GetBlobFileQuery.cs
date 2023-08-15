using BaseModule.Application.DTOs.Responses;
using BlobModule.Domain.Entities;
using MediatR;

namespace BlobModule.Application.Queries
{
    public class GetBlobFileQuery : IRequest<QueryRecordResponse<BlobFile>>
    {
        public string FileId { get; set; } = string.Empty;
    }
}
