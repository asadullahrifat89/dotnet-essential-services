using BaseCore.Models.Responses;
using BlobCore.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobCore.Declarations.Queries
{
    public class GetBlobFileQuery: IRequest<QueryRecordResponse<BlobFile>>
    {
        public string FileId { get; set; } = string.Empty;
    }
}
