using BaseCore.Models.Responses;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Queries;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Repositories
{
    public interface IProjectRepository
    {
        Task<ServiceResponse> AddProject(AddProjectCommand command);

        Task<bool> BeAnExistingProject(string projectId);

        Task<QueryRecordResponse<Project>> GetProject(GetProjectQuery query);

        Task<ServiceResponse> UpdateProject(UpdateProjectCommand command);
    }
}
