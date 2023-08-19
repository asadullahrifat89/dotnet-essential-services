using Base.Application.Extensions;
using Base.Infrastructure.Providers.Interfaces;
using Identity.Application.Providers.Interfaces;
using Identity.Domain.Entities;
using MongoDB.Driver;
using Teams.CustomerEngagement.Domain.Entities;
using Teams.CustomerEngagement.Domain.Repositories.Interfaces;

namespace Teams.CustomerEngagement.Infrastructure.Persistence
{
    public class QuotationRepository : IQuotationRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public QuotationRepository(
            IMongoDbContextProvider mongoDbService,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _mongoDbService = mongoDbService;
            _authenticationContextProvider = authenticationContextProvider;
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
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

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

            filter = AddDataContextFilters(filter);

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
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

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

            filter = AddDataContextFilters(filter);

            var count = await _mongoDbService.CountDocuments(filter);

            var quotations = await _mongoDbService.GetDocuments(
                filter: filter,
                skip: pageIndex * pageSize,
                limit: pageSize);

            return (count, quotations is not null ? quotations.ToArray() : Array.Empty<Quotation>());
        }

        public async Task<(long Count, (QuoteStatus QuoteStatus, long Count)[] Records)> GetQuotationStatusCounts()
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var quoteStatusCounts = new List<(QuoteStatus QuoteStatus, long Count)>();

            foreach (QuoteStatus quoteStatus in Enum.GetValues(typeof(QuoteStatus)))
            {
                var filter = Builders<Quotation>.Filter.Eq(x => x.QuoteStatus, quoteStatus);

                filter = AddDataContextFilters(filter);

                var count = await _mongoDbService.CountDocuments(filter);

                quoteStatusCounts.Add((quoteStatus, count));
            }

            return (quoteStatusCounts.Count, quoteStatusCounts.ToArray());
        }

        private FilterDefinition<Quotation> AddDataContextFilters(FilterDefinition<Quotation> filter)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            if (authCtx.User.MetaTags.Contains("customer"))
            {
                filter &= Builders<Quotation>.Filter.Eq(x => x.Email, authCtx.User.Email);
            }

            return filter;
        }

        #endregion
    }
}
