using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Application.Providers.Interfaces;
using MongoDB.Driver;
using ProductSearchCriteriaModule.Domain.Entities;
using ProductSearchCriteriaModule.Domain.Repositories.Interfaces;

namespace ProductSearchCriteriaModule.Infrastructure.Persistence
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

        public async Task<ServiceResponse> AddProductSearchCriteria(ProductSearchCriteria productSearchCriteria)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            await _mongoDbService.InsertDocument(productSearchCriteria);

            return Response.BuildServiceResponse().BuildSuccessResponse(productSearchCriteria, authCtx?.RequestUri);
        }

        public async Task<ServiceResponse> UpdateProductSearchCriteria(ProductSearchCriteria productSearchCriteria)
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

            await _mongoDbService.UpdateById(update: updateSearchCriteria, id: productSearchCriteria.Id);

            var updatedSearchCriteria = await _mongoDbService.FindById<ProductSearchCriteria>(productSearchCriteria.Id);

            return Response.BuildServiceResponse().BuildSuccessResponse(updatedSearchCriteria, authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingProductSearchCriteria(string name)
        {
            var filter = Builders<ProductSearchCriteria>.Filter.Where(x => x.Name.ToLower().Equals(name.ToLower()));

            return await _mongoDbService.Exists(filter);
        }

        public async Task<bool> BeAnExistingProductSearchCriteriaById(string searchCriteriaId)
        {
            var filter = Builders<ProductSearchCriteria>.Filter.Where(x => x.Id == searchCriteriaId);

            var exists = await _mongoDbService.Exists(filter);

            return exists;
        }

        public async Task<QueryRecordResponse<ProductSearchCriteria>> GetProductSearchCriteria(string searchCriteriaId)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<ProductSearchCriteria>.Filter.Where(x => x.Id.Equals(searchCriteriaId));

            var result = await _mongoDbService.FindOne(filter);

            return Response.BuildQueryRecordResponse<ProductSearchCriteria>().BuildSuccessResponse(result, authCtx?.RequestUri);
        }

        public async Task<QueryRecordsResponse<ProductSearchCriteria>> GetProductSearchCriterias(
            string searchTerm,
            int pageIndex,
            int pageSize,
            ProductSearchCriteriaType? searchCriteriaType,
            SkillsetType? skillsetType)
        {
            //TODO: get productid query param and then filter with productsearchcriteriamap

            var authCtx = _authenticationContextProvider.GetAuthenticationContext();
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

            var searchCriterias = await _mongoDbService.GetDocuments(
                filter: filter,
                skip: pageIndex * pageSize,
                limit: pageSize);

            return new QueryRecordsResponse<ProductSearchCriteria>().BuildSuccessResponse(
                count: count,
                records: searchCriterias is not null ? searchCriterias.ToArray() : Array.Empty<ProductSearchCriteria>(), authCtx?.RequestUri);
        }

        #endregion
    }
}
