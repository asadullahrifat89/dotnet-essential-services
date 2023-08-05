using BaseCore.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobCore.Declarations.Commands
{
    public class UploadBlobFileCommand : IRequest<ServiceResponse>
    {
        public IFormFile FormFile { get; set; } = null;
    }
}
