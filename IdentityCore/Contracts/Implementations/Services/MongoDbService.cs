using IdentityCore.Contracts.Declarations.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Implementations.Services
{
    public class MongoDbService : IMongoDbService
    {
        #region Fields

        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly ILogger<MongoDbService> _logger;

        #endregion

        #region Ctor

        public MongoDbService(IConfiguration configuration, ILogger<MongoDbService> logger)
        {
            _databaseName = configuration["ConnectionStrings:DatabaseName"];
            _connectionString = configuration["ConnectionStrings:DatabaseConnectionString"];
            _logger = logger;
        }

        #endregion

        #region Methods

        #region Common

        private IMongoCollection<T> GetMongoCollection<T>()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);

            var collectionName = typeof(T).Name + "s";

            var collection = database.GetCollection<T>(collectionName);
            return collection;
        }

        #endregion

        #region Document

        public async Task<bool> InsertDocument<T>(T document)
        {
            var collection = GetMongoCollection<T>();
            await collection.InsertOneAsync(document);
            return true;
        }

        #endregion

        #endregion
    }
}
