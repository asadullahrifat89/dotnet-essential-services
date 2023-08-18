using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Domain.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<bool> BeAnExistingProject(string projectId);

        Task<Project> AddProject(Project project, string[] linkedProductIds);                

        Task<Project> UpdateProject(Project project, string[] linkedProductIds);

        Task<Project> GetProject(string projectId);

        Task<(long Count, Project[] Records)> GetProjects(
            string searchTerm,
            int pageIndex,
            int pageSize,
            PublishingStatus? publishingStatus);

        Task<(long Count, Project[] Records)> GetProjectsForProductId(string productId);
    }
}
