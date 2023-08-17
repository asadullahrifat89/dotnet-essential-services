using Base.Application.DTOs.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blob.Application.Commands
{
    public class UploadBlobFileCommand : IRequest<ServiceResponse>
    {
        public IFormFile FormFile { get; set; } = null;
    }
}
