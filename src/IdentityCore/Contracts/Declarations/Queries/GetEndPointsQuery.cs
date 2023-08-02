using IdentityCore.Models.Entities;
using IdentityCore.Models.Requests;
using IdentityCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Declarations.Queries
{
    public class GetEndPointsQuery : IRequest<string[]>
    {
       
    }
}
