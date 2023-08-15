using BlobModule.Application.DTOs.Responses;
using MediatR;

namespace BlobModule.Application.Queries
{
    public class DownloadBlobFileQuery : IRequest<BlobFileResponse>
    {
        public string FileId { get; set; } = string.Empty;
    }
}
