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


        public async Task<Product[]>  GetRolesByIds(string[] ids)
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


        #endregion
    }
}
