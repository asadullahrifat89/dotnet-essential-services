using BaseModule.Models.Responses;
using BlobModule.Declarations.Commands;
using BlobModule.Declarations.Queries;
using BlobModule.Models.Entities;
using BlobModule.Models.Responses;

namespace BlobModule.Declarations.Repositories
{
    public interface IBlobFileRepository
    {
        Task<ServiceResponse> UploadBlobFile(UploadBlobFileCommand command);

        Task<BlobFileResponse> DownloadBlobFile(DownloadBlobFileQuery query);

        Task<QueryRecordResponse<BlobFile>> GetBlobFile(GetBlobFileQuery query);

        Task<bool> BeAnExistingBlobFile(string fileId);
    }
}
