using Base.Application.Extensions;
using Base.Infrastructure.Providers.Interfaces;
using Identity.Application.Providers.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using Teams.ContentMangement.Domain.Entities;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Infrastructure.Persistence
{
    public class ProductRepository : IProductRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public ProductRepository(
            IMongoDbContextProvider mongoDbService,
            IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }

        #endregion

        #region Methods

        public async Task<bool> BeAnExistingProductId(string productId)
        {
            var filter = Builders<Product>.Filter.Where(x => x.Id == productId);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<Product> AddProduct(Product product, string[] linkedSearchCriteriaIds)
        {
            await _mongoDbService.InsertDocument(product);

            var productSearchCriteriaMaps = CreateProductSearchCriteriaMaps(linkedSearchCriteriaIds, product.Id);

            if (productSearchCriteriaMaps.Any())
                await _mongoDbService.InsertDocuments(productSearchCriteriaMaps);

            return product;
        }

        public async Task<Product> UpdateProduct(Product product, string[] linkedSearchCriteriaIds)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var update = Builders<Product>.Update
                .Set(x => x.Name, product.Name)
                .Set(x => x.Description, product.Description)
                .Set(x => x.ManPower, product.ManPower)
                .Set(x => x.Experience, product.Experience)
                .Set(x => x.EmploymentTypes, product.EmploymentTypes)
                .Set(x => x.ProductCostType, product.ProductCostType)
                .Set(x => x.IconUrl, product.IconUrl)
                .Set(x => x.PublishingStatus, product.PublishingStatus)
                .Set(x => x.TimeStamp.ModifiedOn, DateTime.UtcNow)
                .Set(x => x.TimeStamp.ModifiedBy, authCtx.User?.Id);

            var deleteFilter = Builders<ProductSearchCriteriaMap>.Filter.Eq(x => x.ProductId, product.Id);
            await _mongoDbService.DeleteDocuments(deleteFilter);

            var newProductSearchCriteriaMaps = CreateProductSearchCriteriaMaps(linkedSearchCriteriaIds, product.Id);
            if (newProductSearchCriteriaMaps.Any())
                await _mongoDbService.InsertDocuments(newProductSearchCriteriaMaps);

            var updateProduct = await _mongoDbService.UpdateById(update, product.Id);

            return updateProduct;
        }

        public async Task<Product> GetProduct(string productId)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, productId);

            var product = await _mongoDbService.FindOne(filter);

            return product;
        }

        public async Task<(long Count, Product[] Records)> GetProducts(
            string searchTerm,
            int pageIndex,
            int pageSize,
            ProductCostType? productCostType,
            EmploymentType? employmentType,
            PublishingStatus? publishingStatus,
            int? manPower,
            int? experience)
        {
            var filter = Builders<Product>.Filter.Empty;

            if (!searchTerm.IsNullOrBlank())
            {
                filter &= Builders<Product>.Filter.Or(
                    Builders<Product>.Filter.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())),
                    Builders<Product>.Filter.Where(x => x.Description.ToLower().Contains(searchTerm.ToLower())));
            }

            if (productCostType.HasValue)
            {
                filter &= Builders<Product>.Filter.Where(x => x.ProductCostType == productCostType);
            }

            if (employmentType.HasValue)
            {
                filter &= Builders<Product>.Filter.ElemMatch(x => x.EmploymentTypes, x => x == employmentType);
            }

            if (publishingStatus.HasValue)
            {
                filter &= Builders<Product>.Filter.Where(x => x.PublishingStatus == publishingStatus);
            }

            if (manPower.HasValue)
            {
                filter &= Builders<Product>.Filter.Gte(x => x.ManPower, manPower);
            }

            if (experience.HasValue)
            {
                filter &= Builders<Product>.Filter.Gte(x => x.Experience, experience);
            }

            var count = await _mongoDbService.CountDocuments(filter);

            var products = await _mongoDbService.GetDocuments(
                filter: filter,
                skip: pageIndex * pageSize,
                limit: pageSize);

            return (count, products is not null ? products.ToArray() : Array.Empty<Product>());
        }

        public async Task<(long Count, (Product product, int MatchCount)[] Records)> GetProductRecommendations(
            int pageIndex,
            int pageSize,
            string[] productSearchCriteriaIds,
            EmploymentType[]? employmentTypes,
            int? manPower,
            int? experience)
        {
            // 1. Fetch ProductSearchCriteriaMaps from ProductSearchCriteriaMaps collection with productSearchCriteriaIds
            var mapsFilter = Builders<ProductSearchCriteriaMap>.Filter.In(x => x.ProductSearchCriteriaId, productSearchCriteriaIds);
            var productSearchCriteriaMaps = await _mongoDbService.GetDocuments(mapsFilter);

            var emptyMatchings = new List<(Product product, int MatchCount)>();

            // if no search criteria mapping was found return empty result
            if (productSearchCriteriaMaps is null || !productSearchCriteriaMaps.Any())
            {
                return (0, emptyMatchings.ToArray());
            }

            var filter = Builders<Product>.Filter.Empty;

            if (employmentTypes is not null && employmentTypes.Any())
            {
                filter &= Builders<Product>.Filter.AnyIn(x => x.EmploymentTypes, employmentTypes);
            }

            if (manPower.HasValue)
            {
                filter &= Builders<Product>.Filter.Gte(x => x.ManPower, manPower);
            }

            if (experience.HasValue)
            {
                filter &= Builders<Product>.Filter.Gte(x => x.Experience, experience);
            }

            // 2. Get productIds from productSearchCriteriaMaps
            // 3. Make a distinct list of productIds
            var productIds = productSearchCriteriaMaps.Select(x => x.ProductId).Distinct().ToArray();


            // 4. Fetch products from Products collection with productIds
            filter &= Builders<Product>.Filter.In(x => x.Id, productIds);
            var count = await _mongoDbService.CountDocuments(filter);

            var products = await _mongoDbService.GetDocuments(
                filter: filter,
                skip: pageIndex * pageSize,
                limit: pageSize);

            // 5. Create in memory tuple list List<(Product Product, int MatchCount)>
            var matchingProducts = new List<(Product Product, int MatchCount)>();
                       
            foreach (var product in products)
            {
                var foundProductSearchCriteriaMaps = productSearchCriteriaMaps.Where(x => x.ProductId == product.Id).ToArray();
                var foundProductSearchCriteriaIds = foundProductSearchCriteriaMaps.Select(x => x.ProductSearchCriteriaId).Distinct().ToArray();

                var matchCount = foundProductSearchCriteriaIds.Intersect(productSearchCriteriaIds).Count();
                matchingProducts.Add((product, matchCount));
            }

            // 6. Re order the matching products with match count
            var orderedResults = matchingProducts.OrderByDescending(x => x.MatchCount).ToArray();

            return (count, orderedResults is not null ? orderedResults : emptyMatchings.ToArray());
        }

        private List<ProductSearchCriteriaMap> CreateProductSearchCriteriaMaps(string[] linkedProductSearchCriteriaIds, string productId)
        {
            var productSearchCriteriaMaps = new List<ProductSearchCriteriaMap>();

            if (linkedProductSearchCriteriaIds != null && linkedProductSearchCriteriaIds.Any())
            {
                var productSearchCriteriaIds = linkedProductSearchCriteriaIds;

                foreach (var productSearchCriteriaId in productSearchCriteriaIds)
                {
                    var productSearchCriteriaMap = new ProductSearchCriteriaMap()
                    {
                        ProductId = productId,
                        ProductSearchCriteriaId = productSearchCriteriaId
                    };

                    productSearchCriteriaMaps.Add(productSearchCriteriaMap);
                }
            }

            return productSearchCriteriaMaps;
        }

        #endregion
    }
}
