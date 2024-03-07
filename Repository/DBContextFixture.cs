using Microsoft.EntityFrameworkCore;
using Models;
using Models.DAO;
using Models.ViewModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DBContextFixture
    {
        private readonly IMongoDatabase _database;

        public DBContextFixture(IMongoClient mongoClient, string databaseName)
        {
            _database = mongoClient.GetDatabase(databaseName);
            Movies = _database.GetCollection<MovieDAO>("Movies");
            Directors = _database.GetCollection<Director>("Directors");
        }

        public IMongoCollection<MovieDAO> Movies { get; }

        public IMongoCollection<Director> Directors { get; }
    }

}
