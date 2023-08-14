using BaseCore.Models.Responses;
using BaseCore.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Models.Entities;

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

        public async Task<ServiceResponse> UpdateProduct(UpdateProductCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<Product>.Filter.Where(x => x.Id == command.ProductId);

            var existingProduct = await _mongoDbService.FindOne(filter);

            var newProductSearchCriteriaMaps = new List<ProductSearchCriteriaMap>();

            if (command.LinkedSearchCriteriaIds != null && command.LinkedSearchCriteriaIds.Any())
            {
                var searchCriteriaIds = command.LinkedSearchCriteriaIds;

                foreach (var searchCriteriaId in searchCriteriaIds)
                {
                    var productSearchCriteriaMap = new ProductSearchCriteriaMap()
                    {
                        ProductId = command.ProductId,
                        SearchCriteriaId = searchCriteriaId
                    };

                    newProductSearchCriteriaMaps.Add(productSearchCriteriaMap);
                }
            }

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

        #endregion
    }
}
