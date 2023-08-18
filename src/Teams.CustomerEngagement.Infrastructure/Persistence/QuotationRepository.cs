using Base.Application.Extensions;
using Base.Infrastructure.Providers.Interfaces;
using Identity.Application.Providers.Interfaces;
using MongoDB.Driver;
using Teams.CustomerEngagement.Domain.Entities;
using Teams.CustomerEngagement.Domain.Repositories.Interfaces;

namespace Teams.CustomerEngagement.Infrastructure.Persistence
{
    public class QuotationRepository : IQuotationRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public QuotationRepository(
            IMongoDbContextProvider mongoDbService,
            IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }

        #endregion

        #region Methods

        public async Task<bool> BeAnExistingQuotationId(string quotationId)
        {
            var filter = Builders<Quotation>.Filter.Where(x => x.Id == quotationId);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<Quotation> AddQuotation(Quotation quotation)
        {
            await _mongoDbService.InsertDocument(quotation);

            return quotation;
        }

        public async Task<Quotation> UpdateQuotation(Quotation quotation)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var update = Builders<Quotation>.Update
                .Set(x => x.QuoteStatus, quotation.QuoteStatus)
                .Set(x => x.Priority, quotation.Priority)
                .Set(x => x.AssignedTeamsUsers, quotation.AssignedTeamsUsers)
                .Set(x => x.LinkedQuotationDocuments, quotation.LinkedQuotationDocuments)
                .Set(x => x.MeetingLink, quotation.MeetingLink)
                .Set(x => x.Note, quotation.Note)
                .Set(x => x.TimeStamp.ModifiedOn, DateTime.UtcNow)
                .Set(x => x.TimeStamp.ModifiedBy, authCtx.User?.Id);

            var updateQuotation = await _mongoDbService.UpdateById(update, quotation.Id);

            return updateQuotation;
        }

        public async Task<Quotation> GetQuotation(string quotationId)
        {
            var filter = Builders<Quotation>.Filter.Eq(x => x.Id, quotationId);

            var product = await _mongoDbService.FindOne(filter);

            return product;
        }

        public async Task<(long Count, Quotation[] Records)> GetQuotations(
            string searchTerm,
            int pageIndex,
            int pageSize,
            Priority? priority,
            DateTime? fromDate,
            DateTime? toDate,
            string? location)
        {
            var filter = Builders<Quotation>.Filter.Empty;

            if (!searchTerm.IsNullOrBlank())
            {
                filter &= Builders<Quotation>.Filter.Or(
                    Builders<Quotation>.Filter.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower())),
                    Builders<Quotation>.Filter.Where(x => x.Note.ToLower().Contains(searchTerm.ToLower())));
            }

            if (priority.HasValue)
            {
                filter &= Builders<Quotation>.Filter.Where(x => x.Priority == priority);
            }

            if (fromDate.HasValue)
            {
                filter &= Builders<Quotation>.Filter.Where(x => x.TimeStamp.CreatedOn >= fromDate);
            }

            if (toDate.HasValue)
            {
                filter &= Builders<Quotation>.Filter.Where(x => x.TimeStamp.CreatedOn <= toDate);
            }

            if (location is not null && !location.IsNullOrBlank())
            {
                filter &= Builders<Quotation>.Filter.Where(x => x.Location == location);
            }

            var count = await _mongoDbService.CountDocuments(filter);

            var quotations = await _mongoDbService.GetDocuments(
                filter: filter,
                skip: pageIndex * pageSize,
                limit: pageSize);

            return (count, quotations is not null ? quotations.ToArray() : Array.Empty<Quotation>());
        }

        #endregion
    }
}
