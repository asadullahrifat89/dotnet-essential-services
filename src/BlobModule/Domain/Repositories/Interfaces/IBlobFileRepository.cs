using BaseModule.Application.DTOs.Responses;
using BlobModule.Application.Commands;
using BlobModule.Application.DTOs.Responses;
using BlobModule.Application.Queries;
using BlobModule.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BlobModule.Domain.Repositories.Interfaces
{
    public interface IBlobFileRepository
    {
        Task<ServiceResponse> UploadBlobFile(IFormFile formFile);

        Task<BlobFileResponse> DownloadBlobFile(string fileId);

        Task<QueryRecordResponse<BlobFile>> GetBlobFile(string fileId);

        Task<bool> BeAnExistingBlobFile(string fileId);
    }
}
