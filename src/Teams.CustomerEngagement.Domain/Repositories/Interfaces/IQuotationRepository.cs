using Teams.CustomerEngagement.Domain.Entities;

namespace Teams.CustomerEngagement.Domain.Repositories.Interfaces
{
    public interface IQuotationRepository
    {
        Task<bool> BeAnExistingQuotationId(string quotationId);

        Task<Quotation> AddQuotation(Quotation quotation);

        Task<Quotation> UpdateQuotation(Quotation quotation);

        Task<Quotation> GetQuotation(string quotationId);

        Task<(long Count, Quotation[] Records)> GetQuotations(
            string searchTerm,
            int pageIndex,
            int pageSize,
            Priority? priority,
            DateTime? fromDate,
            DateTime? toDate,
            string? location);
    }
}
