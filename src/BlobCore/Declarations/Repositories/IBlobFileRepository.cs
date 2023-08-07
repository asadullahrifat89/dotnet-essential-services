using BaseCore.Models.Responses;
using BlobCore.Declarations.Commands;
using BlobCore.Declarations.Queries;
using BlobCore.Models.Entities;
using BlobCore.Models.Responses;

namespace BlobCore.Declarations.Repositories
{
    public interface IBlobFileRepository
    {
        Task<ServiceResponse> UploadBlobFile(UploadBlobFileCommand command);

        Task<BlobFileResponse> DownloadBlobFile(DownloadBlobFileQuery query);

        Task<QueryRecordResponse<BlobFile>> GetBlobFile(GetBlobFileQuery query);

        Task<bool> BeAnExistingBlobFile(string fileId);
    }
}
