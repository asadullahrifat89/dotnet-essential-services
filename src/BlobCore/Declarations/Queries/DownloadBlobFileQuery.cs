using BlobCore.Models.Responses;
using MediatR;

namespace BlobCore.Declarations.Queries
{
    public class DownloadBlobFileQuery : IRequest<BlobFileResponse>
    {
        public string FileId { get; set; } = string.Empty;
    }
}
