using BaseCore.Models.Responses;
using BaseCore.Services;
using MongoDB.Driver;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Models.Entities;

namespace TeamsCore.Implementations.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;
        private readonly IProductRepository _productRepository;

        #endregion

        #region Ctor

        public ProjectRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContext, IProductRepository productRepository)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
            _productRepository = productRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> AddProject(AddProjectCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var project = Project.Initialize(command, authCtx);

            var productProjectMaps = new List<ProductProjectMap>();


            if (command.LinkedProductIds != null && command.LinkedProductIds.Any())
            {
                //var products = await _productRepository.GetRolesByIds(command.LinkedProductIds);

                var productIds = command.LinkedProductIds;

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

            await _mongoDbService.InsertDocument(project);

            if (productProjectMaps.Any())
                await _mongoDbService.InsertDocuments(productProjectMaps);


            return Response.BuildServiceResponse().BuildSuccessResponse(project, authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingProject(string projectId)
        {
            var filter = Builders<Project>.Filter.Where(x => x.Id == projectId);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<QueryRecordResponse<Project>> GetProject(GetProjectQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<Project>.Filter.Eq(x => x.Id, query.ProjectId);

            var project = await _mongoDbService.FindOne(filter);

            return Response.BuildQueryRecordResponse<Project>().BuildSuccessResponse(project, authCtx?.RequestUri);

        }

        public async Task<QueryRecordsResponse<Project>> GetProjects(GetProjectsQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<Project>.Filter.Where(x => x.Name.ToLower().Contains(query.SearchTerm.ToLower()));

            var count = await _mongoDbService.CountDocuments(filter: filter);

            var projects = await _mongoDbService.GetDocuments(filter: filter, skip: query.PageIndex * query.PageSize, limit: query.PageSize);


            return new QueryRecordsResponse<Project>().BuildSuccessResponse(
               count: count,
               records: projects is not null ? projects.Select(x => Project.Initialize(x)).ToArray() : Array.Empty<Project>(), authCtx?.RequestUri);



        }

        public async Task<ServiceResponse> UpdateProject(UpdateProjectCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var existingProject = await _mongoDbService.FindById<Project>(command.ProjectId);

            var newProductProjectMaps = new List<ProductProjectMap>();

            if (command.LinkedProductIds is not null && command.LinkedProductIds.Any())
            {
                foreach (var productId in command.LinkedProductIds.Distinct())
                {
                    var productProjectMap = new ProductProjectMap()
                    {

                        ProjectId = command.ProjectId,
                        ProductId = productId
                    };

                    newProductProjectMaps.Add(productProjectMap);
                }
            }

            var existingProductProjectMaps = await _mongoDbService.GetDocuments(Builders<ProductProjectMap>.Filter.Eq(x => x.ProjectId, command.ProjectId));

            if (existingProductProjectMaps != null && existingProductProjectMaps.Any())
                await _mongoDbService.DeleteDocuments(Builders<ProductProjectMap>.Filter.In(x => x.Id, existingProductProjectMaps.Select(x => x.Id).ToArray()));

            if (newProductProjectMaps.Any())
                await _mongoDbService.InsertDocuments(newProductProjectMaps);

            var update = Builders<Project>.Update
                .Set(x => x.Name, command.Name)
                .Set(x => x.Description, command.Description)
                .Set(x => x.Link, command.Link)
                .Set(x => x.IconUrl, command.IconUrl)
                .Set(x => x.TimeStamp.ModifiedOn, DateTime.UtcNow)
                .Set(x => x.TimeStamp.ModifiedBy, authCtx.User?.Id);

            var updatedProject = await _mongoDbService.UpdateById(update: update, id: command.ProjectId);

            return Response.BuildServiceResponse().BuildSuccessResponse(updatedProject, authCtx?.RequestUri);
        }

        #endregion
    }
}
