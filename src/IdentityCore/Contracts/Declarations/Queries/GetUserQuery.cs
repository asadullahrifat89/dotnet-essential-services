using IdentityCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Declarations.Queries
{
    public class GetUserQuery : IRequest<QueryRecordResponse<UserResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
