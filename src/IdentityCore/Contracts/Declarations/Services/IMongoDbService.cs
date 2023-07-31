using MongoDB.Driver;
using System.Linq.Expressions;

namespace IdentityCore.Contracts.Declarations.Services
{
    public interface IMongoDbService
    {
        Task<long> CountDocuments<T>(FilterDefinition<T> filter);

        Task<bool> Exists<T>(FilterDefinition<T> filter);

        Task<bool> ExistsById<T>(int id);

        Task<T> FindOne<T>(FilterDefinition<T> filter);

        Task<T> FindOne<T>(FilterDefinition<T> filter, SortOrder sortOrder, string sortFieldName);

        Task<T> FindOne<T>(Expression<Func<T, bool>> expression);

        Task<List<T>> GetDocuments<T>(FilterDefinition<T> filter);

        Task<List<T>> GetDocuments<T>(FilterDefinition<T> filter, int skip, int limit);

        Task<List<T>> GetDocuments<T>(FilterDefinition<T> filter, int skip, int limit, SortOrder sortOrder, string sortFieldName);

        Task<List<T>> GetDocuments<T>(FilterDefinition<T> filter, SortOrder sortOrder, string sortFieldName);

        Task<bool> InsertDocument<T>(T document);

        Task<bool> InsertDocuments<T>(IEnumerable<T> documents);

        Task<T> ReplaceDocument<T>(T document, FilterDefinition<T> filter);

        Task<T> UpdateDocument<T>(UpdateDefinition<T> update, FilterDefinition<T> filter);

        Task<bool> UpdateDocuments<T>(UpdateDefinition<T> update, FilterDefinition<T> filter);

        Task<T> DeleteDocument<T>(FilterDefinition<T> filter);

        Task<bool> DeleteDocuments<T>(FilterDefinition<T> filter);

        Task<bool> UpsertDocument<T>(T document, FilterDefinition<T> filter);

        Task<T> UpdateById<T>(UpdateDefinition<T> update, string id);

        Task<T> FindById<T>(string id);

        Task<T> ReplaceById<T>(T document, string id);

        Task<bool> DeleteById<T>(string id);

        Task<bool> UpsertById<T>(T document, string id);

        Task DropCollection<T>();
    }

    public enum SortOrder
    {
        None,
        Ascending,
        Descending
    }
}
