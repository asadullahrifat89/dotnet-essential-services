using IdentityCore.Declarations.Services;
using IdentityCore.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace IdentityCore.Implementations.Services
{
    public class MongoDbService : IMongoDbService
    {
        #region Fields

        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly ILogger<MongoDbService> _logger;

        #endregion

        #region Ctor

        public MongoDbService(IConfiguration configuration, ILogger<MongoDbService> logger, IAuthenticationContextProvider authenticationContextProvider)
        {
            _databaseName = configuration["ConnectionStrings:DatabaseName"];
            _connectionString = configuration["ConnectionStrings:DatabaseConnectionString"];
            _logger = logger;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        #region Common

        private IMongoCollection<T> GetMongoCollection<T>()
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(authCtx.TenantId);

            var collectionName = typeof(T).Name + "s";

            var collection = database.GetCollection<T>(collectionName);
            return collection;
        }

        #endregion

        #region Document

        public async Task<long> CountDocuments<T>(FilterDefinition<T> filter)
        {
            filter = AddTenantContext(filter);
            var collection = GetMongoCollection<T>();
            var result = await collection.Find(filter).CountDocumentsAsync();
            return result;
        }

        public async Task<bool> Exists<T>(FilterDefinition<T> filter)
        {
            filter = AddTenantContext(filter);
            var collection = GetMongoCollection<T>();
            var result = await collection.Find(filter).FirstOrDefaultAsync();
            return result is not null;
        }

        public async Task<T> FindOne<T>(FilterDefinition<T> filter)
        {
            filter = AddTenantContext(filter);
            var collection = GetMongoCollection<T>();
            var result = await collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        //public async Task<T> FindOne<T>(Expression<Func<T, bool>> expression)
        //{
        //    var collection = GetMongoCollection<T>();
        //    var result = await collection.AsQueryable().Where(expression).FirstOrDefaultAsync();
        //    return result;
        //}

        public async Task<T> FindOne<T>(FilterDefinition<T> filter, SortOrder sortOrder, string sortFieldName)
        {
            filter = AddTenantContext(filter);
            var collection = GetMongoCollection<T>();

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    {
                        var sort = Builders<T>.Sort.Ascending(sortFieldName);
                        var result = await collection.Find(filter).Sort(sort).FirstOrDefaultAsync();
                        return result;
                    }
                case SortOrder.Descending:
                    {
                        var sort = Builders<T>.Sort.Descending(sortFieldName);
                        var result = await collection.Find(filter).Sort(sort).FirstOrDefaultAsync();
                        return result;
                    }
                case SortOrder.None:
                default:
                    {
                        var result = await collection.Find(filter).FirstOrDefaultAsync();
                        return result;
                    }
            }
        }

        public async Task<List<T>> GetDocuments<T>(FilterDefinition<T> filter)
        {
            filter = AddTenantContext(filter);
            var collection = GetMongoCollection<T>();
            var result = await collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task<List<T>> GetDocuments<T>(FilterDefinition<T> filter, int skip, int limit)
        {
            filter = AddTenantContext(filter);
            var collection = GetMongoCollection<T>();
            var result = await collection.Find(filter).Skip(skip).Limit(limit).ToListAsync();
            return result;
        }

        public async Task<List<T>> GetDocuments<T>(FilterDefinition<T> filter, int skip, int limit, SortOrder sortOrder, string sortFieldName)
        {
            filter = AddTenantContext(filter);
            var collection = GetMongoCollection<T>();

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    {
                        var sort = Builders<T>.Sort.Ascending(sortFieldName);
                        var result = await collection.Find(filter).Sort(sort).Skip(skip).Limit(limit).ToListAsync();
                        return result;
                    }
                case SortOrder.Descending:
                    {
                        var sort = Builders<T>.Sort.Descending(sortFieldName);
                        var result = await collection.Find(filter).Sort(sort).Skip(skip).Limit(limit).ToListAsync();
                        return result;
                    }
                case SortOrder.None:
                default:
                    {
                        var result = await collection.Find(filter).Skip(skip).Limit(limit).ToListAsync();
                        return result;
                    }
            }
        }

        public async Task<List<T>> GetDocuments<T>(FilterDefinition<T> filter, SortOrder sortOrder, string sortFieldName)
        {
            filter = AddTenantContext(filter);
            var collection = GetMongoCollection<T>();

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    {
                        var sort = Builders<T>.Sort.Ascending(sortFieldName);
                        var result = await collection.Find(filter).Sort(sort).ToListAsync();
                        return result;
                    }
                case SortOrder.Descending:
                    {
                        var sort = Builders<T>.Sort.Descending(sortFieldName);
                        var result = await collection.Find(filter).Sort(sort).ToListAsync();
                        return result;
                    }
                case SortOrder.None:
                default:
                    {
                        var result = await collection.Find(filter).ToListAsync();
                        return result;
                    }
            }
        }

        public async Task<bool> InsertDocument<T>(T document)
        {
            var collection = GetMongoCollection<T>();
            await collection.InsertOneAsync(document);
            return true;
        }

        public async Task<bool> InsertDocuments<T>(IEnumerable<T> documents)
        {
            var collection = GetMongoCollection<T>();
            await collection.InsertManyAsync(documents);
            return true;
        }

        public async Task<T> ReplaceDocument<T>(T document, FilterDefinition<T> filter)
        {
            var collection = GetMongoCollection<T>();
            var result = await collection.FindOneAndReplaceAsync(filter: filter, replacement: document, options: new FindOneAndReplaceOptions<T>
            {
                ReturnDocument = ReturnDocument.After
            });
            return result;
        }

        public async Task<T> UpdateDocument<T>(UpdateDefinition<T> update, FilterDefinition<T> filter)
        {
            var collection = GetMongoCollection<T>();
            var result = await collection.FindOneAndUpdateAsync(filter: filter, update: update, options: new FindOneAndUpdateOptions<T>
            {
                ReturnDocument = ReturnDocument.After
            });
            return result;
        }

        public async Task<bool> UpdateDocuments<T>(UpdateDefinition<T> update, FilterDefinition<T> filter)
        {
            var collection = GetMongoCollection<T>();
            var result = await collection.UpdateManyAsync(filter: filter, update: update);
            return result.IsAcknowledged;
        }

        public async Task<bool> UpsertDocument<T>(T document, FilterDefinition<T> filter)
        {
            var collection = GetMongoCollection<T>();
            var result = await collection.ReplaceOneAsync(filter: filter, replacement: document, options: new ReplaceOptions() { IsUpsert = true });
            return result is not null && result.IsAcknowledged;
        }

        public async Task<T> DeleteDocument<T>(FilterDefinition<T> filter)
        {
            var collection = GetMongoCollection<T>();
            var result = await collection.FindOneAndDeleteAsync(filter: filter);
            return result;
        }

        public async Task<bool> DeleteDocuments<T>(FilterDefinition<T> filter)
        {
            var collection = GetMongoCollection<T>();
            var result = await collection.DeleteManyAsync(filter);
            return result is not null;
        }

        public async Task<bool> ExistsById<T>(int id)
        {
            var collection = GetMongoCollection<T>();
            var filter = Builders<T>.Filter.Eq("Id", id);
            var result = await collection.Find(filter).FirstOrDefaultAsync();
            return result is not null;
        }

        public async Task<T> FindById<T>(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var collection = GetMongoCollection<T>();
            var result = await collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<T> UpdateById<T>(UpdateDefinition<T> update, string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var collection = GetMongoCollection<T>();
            var result = await collection.FindOneAndUpdateAsync(filter: filter, update: update, options: new FindOneAndUpdateOptions<T>
            {
                ReturnDocument = ReturnDocument.After
            });
            return result;
        }

        public async Task<T> ReplaceById<T>(T document, string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var collection = GetMongoCollection<T>();
            var result = await collection.FindOneAndReplaceAsync(filter: filter, replacement: document, options: new FindOneAndReplaceOptions<T>
            {
                ReturnDocument = ReturnDocument.After
            });
            return result;
        }

        public async Task<bool> DeleteById<T>(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var collection = GetMongoCollection<T>();
            var result = await collection.FindOneAndDeleteAsync(filter);
            return result is not null;
        }

        public async Task<bool> UpsertById<T>(T document, string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var collection = GetMongoCollection<T>();
            var result = await collection.ReplaceOneAsync(filter: filter, replacement: document, options: new ReplaceOptions() { IsUpsert = true });
            return result is not null && result.IsAcknowledged;
        }

        public async Task DropCollection<T>()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);

            var collectionName = typeof(T).Name + "s";

            await database.DropCollectionAsync(collectionName);
        }

        private FilterDefinition<T> AddTenantContext<T>(FilterDefinition<T> filter)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();
            filter &= Builders<T>.Filter.Eq("TenantId", authCtx.TenantId);
            return filter;
        }

        #endregion

        #endregion
    }
}
