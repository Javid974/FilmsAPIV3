using Microsoft.Extensions.Configuration;
using Models;
using Models.DAO;
using Models.ViewModel;
using MongoDB.Driver;

namespace FilmsDatabase
{
    public class FilmsDatabase : IDisposable
    {
        private readonly IMongoDatabase _database;

        public FilmsDatabase(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDB:ConnectionStrings").Value;
            var databaseName = configuration.GetSection("MongoDB:Database").Value;

            // Créer une instance de MongoClient et se connecter à la base de données
            MongoClient client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);

        
        }

      

        public void Dispose()
        {
            // Ne rien faire, car la classe MongoClient implémente déjà IDisposable.
        }
    }
}
