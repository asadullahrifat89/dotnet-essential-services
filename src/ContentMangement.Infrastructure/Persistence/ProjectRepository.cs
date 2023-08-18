using Base.Application.Extensions;
using Base.Application.Providers.Interfaces;
using Identity.Application.Providers.Interfaces;
using MongoDB.Driver;
using Teams.ContentMangement.Domain.Entities;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Infrastructure.Persistence
{
    public class ProjectRepository : IProjectRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public ProjectRepository(
            IMongoDbContextProvider mongoDbService,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _mongoDbService = mongoDbService;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<bool> BeAnExistingProject(string projectId)
        {
            var filter = Builders<Project>.Filter.Where(x => x.Id == projectId);

            var res = await _mongoDbService.Exists(filter);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<Project> AddProject(Project project, string[] linkedProductIds)
        {
            var productProjectMaps = CreateProductProjectMaps(project, linkedProductIds);

            await _mongoDbService.InsertDocument(project);

            if (productProjectMaps.Any())
                await _mongoDbService.InsertDocuments(productProjectMaps);

            return project;
        }

        public async Task<Project> UpdateProject(Project project, string[] linkedProductIds)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var update = Builders<Project>.Update
             .Set(x => x.Name, project.Name)
             .Set(x => x.Description, project.Description)
             .Set(x => x.IconUrl, project.IconUrl)
             .Set(x => x.ProjectLink, project.ProjectLink)
             .Set(x => x.ClientLink, project.ClientLink)
             .Set(x => x.ImageUrls, project.ImageUrls)
             .Set(x => x.PublishingStatus, project.PublishingStatus)
             .Set(x => x.TimeStamp.ModifiedOn, DateTime.UtcNow)
             .Set(x => x.TimeStamp.ModifiedBy, authCtx.User?.Id);

            var updatedProject = await _mongoDbService.UpdateById(update: update, id: project.Id);

            var newProductProjectMaps = CreateProductProjectMaps(project, linkedProductIds);

            await _mongoDbService.DeleteDocuments(Builders<ProductProjectMap>.Filter.Eq(x => x.ProductId, project.Id));

            if (newProductProjectMaps.Any())
                await _mongoDbService.InsertDocuments(newProductProjectMaps);

            return updatedProject;
        }

        public async Task<Project> GetProject(string projectId)
        {
            var filter = Builders<Project>.Filter.Eq(x => x.Id, projectId);

            var project = await _mongoDbService.FindOne(filter);

            return project;
        }

        public async Task<(long Count, Project[] Records)> GetProjects(string searchTerm, int pageIndex, int pageSize, PublishingStatus? publishingStatus)
        {
            var filter = Builders<Project>.Filter.Empty;

            if (!searchTerm.IsNullOrBlank())
                filter &= Builders<Project>.Filter.Or(
                    Builders<Project>.Filter.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())),
                    Builders<Project>.Filter.Where(x => x.Description.ToLower().Contains(searchTerm.ToLower())));

            if (publishingStatus.HasValue)
            {
                filter &= Builders<Project>.Filter.Where(x => x.PublishingStatus == publishingStatus);
            }

            var count = await _mongoDbService.CountDocuments(filter: filter);
            var projects = await _mongoDbService.GetDocuments(filter: filter, skip: pageIndex * pageSize, limit: pageSize);

            return (count, projects is not null ? projects.ToArray() : Array.Empty<Project>());
        }

        private List<ProductProjectMap> CreateProductProjectMaps(Project project, string[] linkedProductIds)
        {
            var productProjectMaps = new List<ProductProjectMap>();

            if (linkedProductIds != null && linkedProductIds.Any())
            {
                var productIds = linkedProductIds.Distinct();

                foreach (var productId in productIds)
                {
                    var productProjectMap = new ProductProjectMap()
                    {
                        ProjectId = project.Id,
                        ProductId = productId
                    };

                    productProjectMaps.Add(productProjectMap);
                }
            }

            return productProjectMaps;
        }

        public async Task<(long Count, Project[] Records)> GetProjectsForProductId(string productId)
        {
            var mapFilter = Builders<ProductProjectMap>.Filter.Eq(x => x.ProductId, productId);

            var productProjectMaps = await _mongoDbService.GetDocuments(filter: mapFilter);

            if (productProjectMaps is not null && productProjectMaps.Any())
            {
                var projectIds = productProjectMaps.Select(x => x.ProjectId).ToArray();

                var projectFilter = Builders<Project>.Filter.In(x => x.Id, projectIds);

                var count = await _mongoDbService.CountDocuments(projectFilter);
                var projects = await _mongoDbService.GetDocuments(filter: projectFilter);

                return (count, projects is not null ? projects.ToArray() : Array.Empty<Project>());
            }
            else
            {
                return (0, Array.Empty<Project>());
            }
        }

        #endregion
    }
}
