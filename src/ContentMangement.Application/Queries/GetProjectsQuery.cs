using Base.Application.DTOs.Requests;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Queries
{
    public class GetProjectsQuery : PagedRequestBase<Project>
    {
        public string SearchTerm { get; set; } = string.Empty;

        public PublishingStatus? PublishingStatus { get; set; } = null;
    }
}
