using Base.Application.DTOs.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Queries
{
    public class GetProductQuery : IRequest<QueryRecordResponse<Product>>
    {
        public string ProductId { get; set; } = string.Empty;
    }
}
