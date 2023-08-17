using Blob.Application.DTOs.Responses;
using MediatR;

namespace Blob.Application.Queries
{
    public class DownloadBlobFileQuery : IRequest<BlobFileResponse>
    {
        public string FileId { get; set; } = string.Empty;
    }
}
