using BaseCore.Models.Requests;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Queries
{
    public class GetProjectsQuery : PagedRequestBase<Project>
    {
        public string? SearchTerm { get; set; } = string.Empty;
    }
}
