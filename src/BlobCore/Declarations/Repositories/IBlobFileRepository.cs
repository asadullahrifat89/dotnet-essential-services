using BaseCore.Models.Responses;
using BlobCore.Declarations.Commands;

namespace BlobCore.Declarations.Repositories
{
    public interface IBlobFileRepository
    {
        Task<ServiceResponse> UploadBlobFile(UploadBlobFileCommand command);
    }
}
