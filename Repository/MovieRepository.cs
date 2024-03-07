using Microsoft.Extensions.Configuration;
using Models.DAO;
using Models.ViewModel;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Interface;
using Models;
using System;

namespace Repository
{


    public class MovieRepository : IMovieRepository
    {
        public MovieRepository(IMongoDbFactory mongoDbFactory)
        {
            _moviesCollection = mongoDbFactory.GetCollection<MovieDAO>("Movies");
        }

        public MovieRepository(DBContextFixture contextFixture)
        {
            _moviesCollection = contextFixture.Movies;
        }


        private readonly IMongoCollection<MovieDAO> _moviesCollection;
        public async Task<MovieDAO> Create(MovieDAO movie)
        {
            if (movie.Id == 0)
            {
                movie.Id = _moviesCollection.AsQueryable().Count() + 1;
            }
            await _moviesCollection.InsertOneAsync(movie);
            return movie;
        }

        public async Task<MightWatchMovieDAO> Create(MightWatchMovieDAO movie)
        {
            await _moviesCollection.InsertOneAsync(movie);
            return movie;
        }

        public async Task Save(List<MovieDAO> movies)
        {
            try
            {
                if (movies == null || movies.Count == 0)
                {
                    throw new ArgumentException("La liste des films est vide ou null.");
                }

                await _moviesCollection.InsertManyAsync(movies);
            }
            catch (System.Exception e)
            {
                // Gérer l'exception selon les besoins de votre application
                throw new Exception(e.Message);
            }
        }

        public async Task Delete(Guid id)
        {
            var filter = Builders<MovieDAO>.Filter.Eq(m => m.Uuid, id);
            try
            {
                await _moviesCollection.DeleteOneAsync(filter);
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
                await _moviesCollection.DeleteManyAsync(Builders<MovieDAO>.Filter.Empty);
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<MovieDAO> Update(MovieDAO movie)
        {
            var filter = Builders<MovieDAO>.Filter.Eq(m => m.Uuid, movie.Uuid);
            try
            {
                await _moviesCollection.ReplaceOneAsync(filter, movie);
                return movie;
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<MovieDAO?> GetById(int id)
        {
            var filter = Builders<MovieDAO>.Filter.Eq(m => m.Id, id);
            try
            {
                return await _moviesCollection.Find(filter).FirstOrDefaultAsync();
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<MovieDAO>> GetAll()
        {
            try
            {
                return await _moviesCollection.Find(_ => true).ToListAsync();
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Movie>> GetByViewingYearsDate(int yearsDate)
        {
            try
            {
                var pipeline = new[]
{
                new BsonDocument("$match", new BsonDocument("ViewingYear", yearsDate)),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Directors" },
                    { "localField", "DirectorsIds" },
                    { "foreignField", "_id" },
                    { "as", "Directors" }
                }),

};

                var bsonMovies = await _moviesCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
                var moviesList = bsonMovies.Select(bson => new Movie
                {
                    Uuid = bson.GetValue("Uuid").AsGuid,
                    Id = bson.GetValue("_id").AsInt32,
                    Title = bson.GetValue("Title").AsString,
                    OriginalTitle = bson.GetValue("OriginalTitle").AsString,
                    Overview = bson.GetValue("Overview").AsString,
                    ReleaseDate = bson.GetValue("ReleaseDate").ToUniversalTime(),
                    ViewingYear = bson.GetValue("ViewingYear").AsInt32,
                    ViewingDate = bson.GetValue("ViewingDate").ToUniversalTime(),
                    PosterPath = (bson.GetValue("PosterPath").IsBsonNull) ? null : bson.GetValue("PosterPath").AsString,
                    BackdropPath = (bson.GetValue("BackdropPath").IsBsonNull) ? null : bson.GetValue("BackdropPath").AsString,
                    ImdbNote = (bson.Contains("ImdbNote") && !bson.GetValue("ImdbNote").IsBsonNull) ? bson.GetValue("ImdbNote").AsDouble : null,
                    SenscritiqueNote = (bson.Contains("SenscritiqueNote") && !bson.GetValue("SenscritiqueNote").IsBsonNull) ? bson.GetValue("SenscritiqueNote").AsDouble : null,
                    Support = bson.GetValue("Support").IsBsonNull ? null : (Support?)bson.GetValue("Support").AsInt32,
                    MightWatch = bson.GetValue("MightWatch").AsBoolean,
                    AdditionalInfos = bson.GetValue("AdditionalInfos").IsBsonNull ? null : bson.GetValue("AdditionalInfos").AsString,
                    Directors = bson.GetValue("Directors").AsBsonArray.Select(directorBson => BsonSerializer.Deserialize<Director>(directorBson.AsBsonDocument)).ToList(),
                    Genres = bson.GetValue("Genres").AsBsonArray.Select(genreBson => BsonSerializer.Deserialize<Genre>(genreBson.AsBsonDocument)).ToList(),
                    Countries = bson.GetValue("Countries").AsBsonArray.Select(countryBson => BsonSerializer.Deserialize<Country>(countryBson.AsBsonDocument)).ToList()
                }).OrderBy(m => m.ViewingDate).ToList();



                return moviesList;
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Movie> GetById(Guid id)
        {
            try
            {
                var guidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
                var pipeline = new[]
                {

                    new BsonDocument("$match", new BsonDocument("Uuid", new BsonBinaryData(id, guidRepresentation))),
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "Directors" },
                        { "localField", "DirectorsIds" },
                        { "foreignField", "_id" },
                        { "as", "Directors" }
                    }),

                };

                var bsonMovie = await _moviesCollection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
                var movie = new Movie
                {
                    Uuid = bsonMovie.GetValue("Uuid").AsGuid,
                    Id = bsonMovie.GetValue("_id").AsInt32,
                    Title = bsonMovie.GetValue("Title").AsString,
                    OriginalTitle = bsonMovie.GetValue("OriginalTitle").AsString,
                    Overview = bsonMovie.GetValue("Overview").AsString,
                    ReleaseDate = bsonMovie.GetValue("ReleaseDate").ToUniversalTime(),
                    ViewingDate = bsonMovie.GetValue("ViewingDate").ToUniversalTime(),
                    PosterPath = bsonMovie.GetValue("PosterPath").IsBsonNull ? null : bsonMovie.GetValue("PosterPath").AsString,
                    BackdropPath = bsonMovie.GetValue("BackdropPath").IsBsonNull ? null : bsonMovie.GetValue("BackdropPath").AsString,
                    MightWatch = bsonMovie.GetValue("MightWatch").AsBoolean,
                    Support = bsonMovie.GetValue("Support").IsBsonNull ? null : (Support?)bsonMovie.GetValue("Support").AsInt32,
                    Directors = bsonMovie.GetValue("Directors").AsBsonArray.Select(directorBson => BsonSerializer.Deserialize<Director>(directorBson.AsBsonDocument)).ToList(),
                    Genres = bsonMovie.GetValue("Genres").AsBsonArray.Select(genreBson => BsonSerializer.Deserialize<Genre>(genreBson.AsBsonDocument)).ToList(),
                    Countries = bsonMovie.GetValue("Countries").AsBsonArray.Select(countryBson => BsonSerializer.Deserialize<Country>(countryBson.AsBsonDocument)).ToList()
                };
                return movie;
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<DirectorMoviesCount>> GetDirectorMoviesCount()
        {
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument
                {
                    { "MightWatch", false }
                }),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Directors" },
                    { "localField", "DirectorsIds" },
                    { "foreignField", "_id" },
                    { "as", "Directors" }
                }),
                new BsonDocument("$unwind", "$Directors"),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$Directors._id" }, // group by director id
                    { "name", new BsonDocument("$first", "$Directors.Name") }, // take the name of the director
                    { "count", new BsonDocument("$sum", 1) }, // count the documents for each group
                })
            };
            var bsonMovies = await _moviesCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
            var directorMoviesCount = bsonMovies.Select(bson => new DirectorMoviesCount
            {
                DirectorId = bson.GetValue("_id").AsInt32,
                DirectorName = bson.GetValue("name").AsString,
                Count = bson.GetValue("count").AsInt32
            }).OrderByDescending(d => d.Count).ThenBy(d => d.DirectorName).ToList();


            return directorMoviesCount;
        }

        public async Task<List<Movie>> GetByDirectorId(int directorId)
        {
            var pipeline = new[]
            {
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Directors" },
                    { "localField", "DirectorsIds" },
                    { "foreignField", "_id" },
                    { "as", "Directors" }
                }),
                new BsonDocument("$unwind", "$Directors"),
                new BsonDocument("$match", new BsonDocument("Directors._id", directorId))
            };

            var bsonMovies = await _moviesCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
            var moviesList = bsonMovies.Select(bson => new Movie
            {
                // Mapez ici le reste de vos champs Movie
                Uuid = bson.GetValue("Uuid").AsGuid,
                Id = bson.GetValue("_id").AsInt32,
                Title = bson.GetValue("Title").AsString,
                OriginalTitle = bson.GetValue("OriginalTitle").AsString,
                Overview = bson.GetValue("Overview").AsString,
                ReleaseDate = bson.GetValue("ReleaseDate").ToUniversalTime(),
                PosterPath = bson.GetValue("PosterPath").AsString,
                BackdropPath = (bson.GetValue("BackdropPath").IsBsonNull) ? null : bson.GetValue("BackdropPath").AsString,
                Genres = bson.GetValue("Genres").AsBsonArray.Select(genreBson => BsonSerializer.Deserialize<Genre>(genreBson.AsBsonDocument)).ToList(),
                // ...
            }).OrderByDescending(m => m.ReleaseDate).ToList();

            return moviesList;
        }

    }
}