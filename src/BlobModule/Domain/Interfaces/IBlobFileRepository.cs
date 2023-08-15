using BaseModule.Application.DTOs.Responses;
using BlobModule.Application.Commands;
using BlobModule.Application.DTOs.Responses;
using BlobModule.Application.Queries;
using BlobModule.Domain.Entities;

namespace BlobModule.Domain.Interfaces
{
    public interface IBlobFileRepository
    {
        Task<ServiceResponse> UploadBlobFile(UploadBlobFileCommand command);

        Task<BlobFileResponse> DownloadBlobFile(DownloadBlobFileQuery query);

        Task<QueryRecordResponse<BlobFile>> GetBlobFile(GetBlobFileQuery query);

        Task<bool> BeAnExistingBlobFile(string fileId);
    }
}
