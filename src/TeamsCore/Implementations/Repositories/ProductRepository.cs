using BaseCore.Models.Responses;
using BaseCore.Services;
using MongoDB.Driver;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Models.Entities;
using TeamsCore.Models.Responses;

namespace TeamsCore.Implementations.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public ProductRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }

        #endregion

        #region Methods


        public async Task<Product[]> GetRolesByIds(string[] ids)
        {
            var filter = Builders<Product>.Filter.In(x => x.Id, ids);

            var results = await _mongoDbService.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<Product>();
        }

        public async Task<bool> BeAnExistingProductId(string productId)
        {
            var filter = Builders<Product>.Filter.Where(x => x.Id == productId);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<ServiceResponse> AddProduct(AddProductCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var product = Product.Initialize(command, authCtx);

            var productSearchCriteriaMaps = new List<ProductSearchCriteriaMap>();

            if (command.LinkedSearchCriteriaIds != null && command.LinkedSearchCriteriaIds.Any())
            {
                var searchCriteriaIds = command.LinkedSearchCriteriaIds;

                foreach (var searchCriteriaId in searchCriteriaIds)
                {
                    var productProjectMap = new ProductSearchCriteriaMap()
                    {
                        ProductId = product.Id,
                        SearchCriteriaId = searchCriteriaId
                    };

                    productSearchCriteriaMaps.Add(productProjectMap);
                }
            }

            await _mongoDbService.InsertDocument(product);

            if (productSearchCriteriaMaps.Any())
                await _mongoDbService.InsertDocuments(productSearchCriteriaMaps);

            return Response.BuildServiceResponse().BuildSuccessResponse(product, authCtx?.RequestUri);
        }

        public async Task<QueryRecordResponse<ProductResponse>> GetProduct(GetProductQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<Product>.Filter.Eq(x => x.Id, query.ProductId);

            var product = await _mongoDbService.FindOne(filter);

            var productResponse = new ProductResponse();

            productResponse.Name = product.Name;
            productResponse.Description = product.Description;
            productResponse.ManPower = product.ManPower;
            productResponse.Experience = product.Experience;
            productResponse.EmploymentType = product.EmploymentType;
            productResponse.ProductCostType = product.ProductCostType;
            productResponse.IconUrl = product.IconUrl;
            productResponse.BannerUrl = product.BannerUrl;
            await RetriveAttachedSearchCriterias(query, productResponse);
            await RetriveAttachtedProjects(query, productResponse);

            return Response.BuildQueryRecordResponse<ProductResponse>().BuildSuccessResponse(productResponse, authCtx?.RequestUri);
        }

        private async Task RetriveAttachtedProjects(GetProductQuery query, ProductResponse productResponse)
        {
            var projectFilter = Builders<ProductProjectMap>.Filter.Eq(x => x.ProductId, query.ProductId);
            var productProjectMaps = await _mongoDbService.GetDocuments(filter: projectFilter);

            if (productProjectMaps.Any())
            {
                var projectIds = productProjectMaps.Select(x => x.ProjectId).ToArray();

                var projectFilter2 = Builders<Project>.Filter.In(x => x.Id, projectIds);

                var projects = await _mongoDbService.GetDocuments(filter: projectFilter2);

                if (projects.Any())
                {
                    productResponse.AttachedProjects = projects.Select(x => new AttachedProject()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IconUrl = x.IconUrl,
                        Description = x.Description,
                        Link = x.Link
                    }).ToArray();
                }
            }
        }

        private async Task RetriveAttachedSearchCriterias(GetProductQuery query, ProductResponse productResponse)
        {
            var searchCriteriaFilter = Builders<ProductSearchCriteriaMap>.Filter.Eq(x => x.ProductId, query.ProductId);
            var searchCriteriaMaps = await _mongoDbService.GetDocuments(filter: searchCriteriaFilter);

            if (searchCriteriaMaps.Any())
            {
                var searchCriteriaIds = searchCriteriaMaps.Select(x => x.SearchCriteriaId).ToArray();

                var searchCriteriaFilter2 = Builders<SearchCriteria>.Filter.In(x => x.Id, searchCriteriaIds);

                var searchCriterias = await _mongoDbService.GetDocuments(filter: searchCriteriaFilter2);

                if (searchCriterias.Any())
                {
                    productResponse.AttachedSearchCriterias = searchCriterias.Select(x => new AttachedSearchCriteria()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IconUrl = x.IconUrl
                    }).ToArray();
                }
            }
        }


        #endregion
    }
}
