using System;
using MongoDB.Driver;

namespace VUTTR.Backend.Data
{
    public class MongoFactory
    {
        private readonly string _connectionString;

        private readonly string _databaseName;
        private string v;
        private object p;

        public MongoFactory(string connectionString, string databaseName)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        }

        private IMongoClient GetClient()
        {
            var client = new MongoClient(_connectionString);
            return client;
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return GetClient().GetDatabase(_databaseName).GetCollection<T>(name);
        }
    }
}
