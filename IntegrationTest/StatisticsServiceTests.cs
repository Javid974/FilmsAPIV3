using Models.DAO;
using Mongo2Go;
using Repository.Interface;
using Repository;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Models;

namespace IntegrationTest
{
    public class StatisticsServiceTests : IDisposable
    {

        private StatisticsService _statsServices;
        private readonly MongoDbRunner _mongoDbRunner;
        private readonly DBContextFixture _context;
        private readonly IMovieRepository _movieRepository;
        private readonly IDirectorRepository _directorRepository;
        private MovieDAO _newMovie;
        public StatisticsServiceTests()
        {
            _mongoDbRunner = MongoDbRunner.Start();
            var client = new MongoClient(_mongoDbRunner.ConnectionString);

            _context = new DBContextFixture(client, "IntegrationTest");
            _movieRepository = new MovieRepository(_context);
            _directorRepository = new DirectorRepository(_context);
            _statsServices = new StatisticsService(_movieRepository, _directorRepository);

            _newMovie = new MovieDAO
            {
                Uuid = Guid.NewGuid(),
                Id = 4,
                DirectorsIds = new List<int>
                {
                    1,
                    2
                },
                Title = "Film 1",
                OriginalTitle = "Film 1",
                Overview = "Synopsis du film 1",
                ReleaseDate = new DateTime(2023, 2, 2),
                PosterPath = "/path/to/poster2.jpg",
                BackdropPath = "/path/to/backdrop2.jpg",
                Support = Support.None,
                MightWatch = false,
                Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America" } }
            };
            _context.Movies.InsertOne(_newMovie);
            var _newMovie2 = new MightWatchMovieDAO
            {
                Uuid = Guid.NewGuid(),
                Id = 5,
                DirectorsIds = new List<int>
                {
                    1,
                    2
                },
                Title = "Film 2",
                OriginalTitle = "Film 2",
                Overview = "Synopsis du film 2",
                ReleaseDate = new DateTime(2023, 2, 2),
                PosterPath = "/path/to/poster2.jpg",
                BackdropPath = "/path/to/backdrop2.jpg",
                Support = Support.None,
                MightWatch = true,
                ImdbNote = 5,
                Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America" } }
            };
            _context.Movies.InsertOne(_newMovie2);
            var _newMovie3 = new MightWatchMovieDAO
            {
                Uuid = Guid.NewGuid(),
                Id = 6,
                DirectorsIds = new List<int>
                {
                    1,
                    2
                },
                Title = "Film 3",
                OriginalTitle = "Film 3",
                Overview = "Synopsis du film 3",
                ReleaseDate = new DateTime(2023, 2, 2),
                PosterPath = "/path/to/poster3.jpg",
                BackdropPath = "/path/to/backdrop3.jpg",
                Support = Support.None,
                MightWatch = true,
                ImdbNote = 5,
                Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                Countries = new List<Country> { new Country { Iso_3166_1 = "FR", English_name = "France", Native_name = "France" }, new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America" } }
            };
            _context.Movies.InsertOne(_newMovie3);
            _context.Directors.InsertOne(new Director() { Id = 1, Name = "test" });
            _context.Directors.InsertOne(new Director() { Id = 2, Name = "test2" });
        }
        [Fact]
        public async Task Get_ReturnsCorrectStats()
        {
            var actualMovies = await _statsServices.Get(2023);
            Assert.NotNull(actualMovies);
        }

        public void Dispose()
        {
            _mongoDbRunner.Dispose();
        }
    }
}
