using BaseCore.Models.Requests;
using BaseCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Queries
{
    public class GetProjectsQuery : PagedRequestBase<Project>
    {
        public string? SearchTerm { get; set; } = string.Empty;
    }
}
