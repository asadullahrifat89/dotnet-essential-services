using BaseCore.Models.Responses;
using MediatR;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Queries
{
    public class GetProjectQuery : IRequest<QueryRecordResponse<Project>>
    {
        public string ProjectId { get; set; } = string.Empty;
    }
}
