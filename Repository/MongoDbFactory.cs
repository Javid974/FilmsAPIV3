using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MongoDbFactory : IMongoDbFactory
    {
        private readonly IMongoDatabase _database;

        public MongoDbFactory(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDB:ConnectionStrings").Value;
            var databaseName = configuration.GetSection("MongoDB:Database").Value;
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        
          
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }

    public interface IMongoDbFactory
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }

}
