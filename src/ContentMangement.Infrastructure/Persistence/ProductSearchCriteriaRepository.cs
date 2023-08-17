using Base.Application.Extensions;
using Base.Application.Providers.Interfaces;
using ContentMangement.Domain.Entities;
using ContentMangement.Domain.Repositories.Interfaces;
using Identity.Application.Providers.Interfaces;
using MongoDB.Driver;

namespace ContentMangement.Infrastructure.Persistence
{
    public class ProductSearchCriteriaRepository : IProductSearchCriteriaRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public ProductSearchCriteriaRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContextProvider)
        {
            _mongoDbService = mongoDbService;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ProductSearchCriteria> AddProductSearchCriteria(ProductSearchCriteria productSearchCriteria)
        {
            await _mongoDbService.InsertDocument(productSearchCriteria);

            return productSearchCriteria;
        }

        public async Task<bool> BeAnExistingProductSearchCriteria(string name)
        {
            var filter = Builders<ProductSearchCriteria>.Filter.Where(x => x.Name.ToLower().Equals(name.ToLower()));

            return await _mongoDbService.Exists(filter);
        }

        public async Task<bool> BeAnExistingProductSearchCriteriaById(string id)
        {
            var filter = Builders<ProductSearchCriteria>.Filter.Where(x => x.Id == id);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<ProductSearchCriteria> GetProductSearchCriteria(string id)
        {
            var filter = Builders<ProductSearchCriteria>.Filter.Where(x => x.Id.Equals(id));

            return await _mongoDbService.FindOne(filter);
        }

        public async Task<(long Count, ProductSearchCriteria[] Records)> GetProductSearchCriterias(
            string searchTerm,
            int pageIndex,
            int pageSize,
            SearchCriteriaType? searchCriteriaType,
            SkillsetType? skillsetType)
        {
            var filter = Builders<ProductSearchCriteria>.Filter.Empty;

            if (!searchTerm.IsNullOrBlank())
            {
                filter &= Builders<ProductSearchCriteria>.Filter.Or(
                    Builders<ProductSearchCriteria>.Filter.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())),
                    Builders<ProductSearchCriteria>.Filter.Where(x => x.Description.ToLower().Contains(searchTerm.ToLower())));
            }

            if (searchCriteriaType.HasValue)
            {
                filter &= Builders<ProductSearchCriteria>.Filter.Eq(x => x.SearchCriteriaType, searchCriteriaType);
            }

            if (skillsetType.HasValue)
            {
                filter &= Builders<ProductSearchCriteria>.Filter.Eq(x => x.SkillsetType, skillsetType);
            }

            var count = await _mongoDbService.CountDocuments(filter: filter);

            var records = await _mongoDbService.GetDocuments(
                filter: filter,
                skip: pageIndex * pageSize,
                limit: pageSize);

            return (count, records is not null ? records.ToArray() : Array.Empty<ProductSearchCriteria>());
        }

        public async Task<ProductSearchCriteria> UpdateProductSearchCriteria(ProductSearchCriteria productSearchCriteria)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var updateSearchCriteria = Builders<ProductSearchCriteria>.Update
                .Set(x => x.Name, productSearchCriteria.Name)
                .Set(x => x.Description, productSearchCriteria.Description)
                .Set(x => x.IconUrl, productSearchCriteria.IconUrl)
                .Set(x => x.SkillsetType, productSearchCriteria.SkillsetType)
                .Set(x => x.SearchCriteriaType, productSearchCriteria.SearchCriteriaType)
                .Set(x => x.TimeStamp.ModifiedOn, DateTime.UtcNow)
                .Set(x => x.TimeStamp.ModifiedBy, authCtx.User?.Id);

            var updatedSearchCriteria = await _mongoDbService.UpdateById(update: updateSearchCriteria, id: productSearchCriteria.Id);

            return updatedSearchCriteria;
        }

        #endregion
    }
}
