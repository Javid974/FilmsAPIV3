using Microsoft.Extensions.Configuration;
using Models;
using Models.DAO;
using Models.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.CustomException;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly IMongoCollection<Director> _directorsCollection;

        public DirectorRepository(IMongoDbFactory mongoDbFactory)
        {
            _directorsCollection = mongoDbFactory.GetCollection<Director>("Directors");
        }

        public DirectorRepository(DBContextFixture contextFixture)
        {
            _directorsCollection = contextFixture.Directors;
        }

        public async Task<List<Director>> GetAll()
        {
            var filter = new BsonDocument(); // Aucun filtre, récupère tous les documents
            return await _directorsCollection.Find(filter).ToListAsync();
        }
        public Task<Director> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAll()
        {
            // Supprime tous les documents dans la collection 'Directors'.
            await _directorsCollection.DeleteManyAsync(Builders<Director>.Filter.Empty);
        }

        public async Task<Director?> GetByName(string name)
        {
            try
            {
                var filter = Builders<Director>.Filter.Eq(m => m.Name, name);
                var director = await _directorsCollection.Find(filter).FirstOrDefaultAsync();
                return director;
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<Director?> GetById(int id)
        {
            try
            {
                var filter = Builders<Director>.Filter.Eq(m => m.Id, id);
                var director = await _directorsCollection.Find(filter).FirstOrDefaultAsync();
                return director;
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<Director> Save(Director director)
        {
            await _directorsCollection.InsertOneAsync(director);
            return director;
        }

        public async Task Save(List<Director> directors)
        {
            try
            {
                if (directors == null || directors.Count == 0)
                {
                    throw new ArgumentException("La liste des films est vide ou null.");
                }

                await _directorsCollection.InsertManyAsync(directors);
            }
            catch (System.Exception e)
            {
                // Gérer l'exception selon les besoins de votre application
                throw new Exception(e.Message);
            }
        }

        public async Task<Director> Update(Director director)
        {
            var filter = Builders<Director>.Filter.Eq(d => d.Id, director.Id);

            var updateResult = await _directorsCollection.ReplaceOneAsync(filter, director);

            if (updateResult.ModifiedCount == 0)
            {
                throw new NotFoundException("The director doesn't exist.");
            }

            return director;
        }

    



    }
}
