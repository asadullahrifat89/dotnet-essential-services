using BaseCore.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlobCore.Declarations.Commands
{
    public class UploadBlobFileCommand : IRequest<ServiceResponse>
    {
        public IFormFile FormFile { get; set; } = null;
    }
}
