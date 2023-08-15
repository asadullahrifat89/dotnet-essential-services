using BaseModule.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlobModule.Declarations.Commands
{
    public class UploadBlobFileCommand : IRequest<ServiceResponse>
    {
        public IFormFile FormFile { get; set; } = null;
    }
}
