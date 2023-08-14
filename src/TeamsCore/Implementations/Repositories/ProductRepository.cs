using BaseCore.Extensions;
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

            var productSearchCriteriaMaps = CreateProductSearchCriteriaMaps(command.LinkedSearchCriteriaIds, product.Id);

            await _mongoDbService.InsertDocument(product);

            if (productSearchCriteriaMaps.Any())
                await _mongoDbService.InsertDocuments(productSearchCriteriaMaps);

            return Response.BuildServiceResponse().BuildSuccessResponse(product, authCtx?.RequestUri);
        }

        public async Task<ServiceResponse> UpdateProduct(UpdateProductCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<Product>.Filter.Where(x => x.Id == command.ProductId);

            var existingProduct = await _mongoDbService.FindOne(filter);

            var newProductSearchCriteriaMaps = CreateProductSearchCriteriaMaps(command.LinkedSearchCriteriaIds, command.ProductId);

            var existingProductSearchCriteriaMaps = await _mongoDbService.GetDocuments(filter);

            if (existingProductSearchCriteriaMaps is not null && existingProductSearchCriteriaMaps.Any())
            {
                var existingProductSearchCriteriaMapIds = existingProductSearchCriteriaMaps.Select(x => x.Id).ToArray();

                var deleteFilter = Builders<ProductSearchCriteriaMap>.Filter.In(x => x.Id, existingProductSearchCriteriaMapIds);

                await _mongoDbService.DeleteDocuments(deleteFilter);
            }

            if (newProductSearchCriteriaMaps.Any())
                await _mongoDbService.InsertDocuments(newProductSearchCriteriaMaps);

            var update = Builders<Product>.Update
                .Set(x => x.Name, command.Name)
                .Set(x => x.Description, command.Description)
                .Set(x => x.ManPower, command.ManPower)
                .Set(x => x.Experience, command.Experience)
                .Set(x => x.EmploymentType, command.EmploymentType)
                .Set(x => x.ProductCostType, command.ProductCostType)
                .Set(x => x.IconUrl, command.IconUrl)
                .Set(x => x.BannerUrl, command.BannerUrl)
                .Set(x => x.TimeStamp.ModifiedOn, DateTime.UtcNow)
                .Set(x => x.TimeStamp.ModifiedBy, authCtx.User?.Id);

            var updateProduct = await _mongoDbService.UpdateById(update, existingProduct.Id);

            return Response.BuildServiceResponse().BuildSuccessResponse(updateProduct, authCtx?.RequestUri);
        }

        private List<ProductSearchCriteriaMap> CreateProductSearchCriteriaMaps(string[] LinkedSearchCriteriaIds, string ProductId)
        {
            var productSearchCriteriaMaps = new List<ProductSearchCriteriaMap>();

            if (LinkedSearchCriteriaIds != null && LinkedSearchCriteriaIds.Any())
            {
                var searchCriteriaIds = LinkedSearchCriteriaIds;

                foreach (var searchCriteriaId in searchCriteriaIds)
                {
                    var productSearchCriteriaMap = new ProductSearchCriteriaMap()
                    {
                        ProductId = ProductId,
                        SearchCriteriaId = searchCriteriaId
                    };

                    productSearchCriteriaMaps.Add(productSearchCriteriaMap);
                }
            }

            return productSearchCriteriaMaps;
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

        public async Task<QueryRecordsResponse<ProductResponse>> GetProducts(GetProductsQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<Product>.Filter.Empty;

            if (!query.SearchTerm.IsNullOrBlank())
            {
                filter &= Builders<Product>.Filter.Where(
                    x => x.Name.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            if (query.ProductCostType.HasValue)
            {
                  filter &= Builders<Product>.Filter.Where(
                                         x => x.ProductCostType == query.ProductCostType);
            }

            var count = await _mongoDbService.CountDocuments(filter);

            var products = await _mongoDbService.GetDocuments(filter: filter, skip: query.PageIndex * query.PageSize, limit: query.PageSize);

            var productResponses = new List<ProductResponse>();

            foreach (var product in products)
            {
                var productResponse = new ProductResponse();
                productResponse.Id = product.Id;
                productResponse.Name = product.Name;
                productResponse.Description = product.Description;
                productResponse.ManPower = product.ManPower;
                productResponse.Experience = product.Experience;
                productResponse.EmploymentType = product.EmploymentType;
                productResponse.ProductCostType = product.ProductCostType;
                productResponse.IconUrl = product.IconUrl;
                productResponse.BannerUrl = product.BannerUrl;

                await RetriveAttachedSearchCriterias(productResponse);
                await RetriveAttachtedProjects(productResponse);

                productResponses.Add(productResponse);
            }

            return Response.BuildQueryRecordsResponse<ProductResponse>().BuildSuccessResponse(count: count, records: productResponses.ToArray(), authCtx?.RequestUri);
        }

        private async Task RetriveAttachtedProjects(ProductResponse productResponse)
        {
            var projectFilter = Builders<ProductProjectMap>.Filter.Eq(x => x.ProductId, productResponse.Id);
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

        private async Task RetriveAttachedSearchCriterias(ProductResponse productResponse)
        {
            var searchCriteriaFilter = Builders<ProductSearchCriteriaMap>.Filter.Eq(x => x.ProductId, productResponse.Id);
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
