using Blob.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Blob.Domain.Repositories.Interfaces
{
    public interface IBlobFileRepository
    {
        Task<BlobFile> UploadBlobFile(IFormFile formFile);

        Task<(BlobFile blobFile, byte[] bytes)> DownloadBlobFile(string fileId);

        Task<BlobFile> GetBlobFile(string fileId);

        Task<bool> BeAnExistingBlobFile(string fileId);
    }
}
