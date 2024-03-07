using Models;
using Models.DAO;
using Models.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TopMovieRepository : ITopMovieRepository
    {
        private readonly IMongoCollection<TopMovie> _topMoviesCollection;
        public TopMovieRepository(IMongoDbFactory mongoDbFactory)
        {
            _topMoviesCollection = mongoDbFactory.GetCollection<TopMovie>("TopMovies");
        }

        public async Task<TopMovie> Create(TopMovie movie)
        {
            await _topMoviesCollection.InsertOneAsync(movie);
            return movie;
        }

        public async Task Delete(Guid id)
        {
            var filter = Builders<TopMovie>.Filter.Eq(m => m.Id, id);
            try
            {
                await _topMoviesCollection.DeleteOneAsync(filter);
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task DeleteAll()
        {
            try
            {
                await _topMoviesCollection.DeleteManyAsync(Builders<TopMovie>.Filter.Empty);
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TopMovie> Get(Guid id)
        {
            return await _topMoviesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<TopMovie>> GetAll()
        {
            return await _topMoviesCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<List<TopMovie>> GetByDate(int yearsDate)
        {
            return await _topMoviesCollection.Find(x => x.Years == yearsDate).ToListAsync();
        }

        public async Task Save(List<TopMovie> topMovies)
        {
            try
            {
                if (topMovies == null || topMovies.Count == 0)
                {
                    throw new ArgumentException("La liste des films est vide ou null.");
                }

                await _topMoviesCollection.InsertManyAsync(topMovies);
            }
            catch (System.Exception e)
            {
                // Gérer l'exception selon les besoins de votre application
                throw new Exception(e.Message);
            }
        }

        public async Task<TopMovie> Update(TopMovie movie)
        {
            var filter = Builders<TopMovie>.Filter.Eq(m => m.Id, movie.Id);
            await _topMoviesCollection.ReplaceOneAsync(filter, movie);
            return movie;
        }
    }
}
