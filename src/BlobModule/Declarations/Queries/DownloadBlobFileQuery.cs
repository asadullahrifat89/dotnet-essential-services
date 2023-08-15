using BlobModule.Models.Responses;
using MediatR;

namespace BlobModule.Declarations.Queries
{
    public class DownloadBlobFileQuery : IRequest<BlobFileResponse>
    {
        public string FileId { get; set; } = string.Empty;
    }
}
