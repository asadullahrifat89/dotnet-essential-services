using Base.Application.Providers.Interfaces;
using Identity.Application.Providers.Interfaces;
using Teams.ContentMangement.Domain.Entities;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Infrastructure.Persistence
{
    public class ProjectRepository : IProjectRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public ProjectRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }

        #endregion

        #region Methods

        public Task<Project> AddProject(Project project, string[] linkedProductIds)
        {
            throw new NotImplementedException();
        }

        public Task<bool> BeAnExistingProject(string projectId, string[] linkedProductIds)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProject(string projectId)
        {
            throw new NotImplementedException();
        }

        public Task<(long Count, Project[] Records)> GetProjects(string searchTerm, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Project> UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
