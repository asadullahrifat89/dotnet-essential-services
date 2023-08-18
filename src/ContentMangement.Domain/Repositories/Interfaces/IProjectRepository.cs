using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Domain.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<bool> BeAnExistingProject(string projectId, string[] linkedProductIds);

        Task<Project> AddProject(Project project, string[] linkedProductIds);                

        Task<Project> UpdateProject(Project project);

        Task<Project> GetProject(string projectId);

        Task<(long Count, Project[] Records)> GetProjects(string searchTerm, int pageIndex, int pageSize);
    }
}
