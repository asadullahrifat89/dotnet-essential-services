using BaseCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Models.Entities;
using TeamsCore.Models.Responses;

namespace TeamsCore.Declarations.Queries
{
    public class GetProductQuery : IRequest<QueryRecordResponse<ProductResponse>>
    {
        public string ProductId { get; set; } = string.Empty;
    }
}
