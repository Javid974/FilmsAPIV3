
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Models;
using Models.DAO;
using Models.ViewModel;
using Mongo2Go;
using MongoDB.Driver;
using Repository;
using Repository.Interface;
using Services;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Services.Helpers;
using Amazon.Runtime;
using Moq;
using Microsoft.Extensions.Options;
using DocumentFormat.OpenXml.Wordprocessing;

namespace IntegrationTest
{
    public class MovieIntegrationTests : IDisposable
    {

        private MovieService _movieService;
        private readonly MongoDbRunner _mongoDbRunner;
        private readonly DBContextFixture _context;
        private readonly IMovieRepository _movieRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly ILogger<MovieService> _logger;
        private MovieDAO _newMovie;
        private readonly TMDbService _tmdbService;


        public MovieIntegrationTests()
        {
            var settings = new ApiSettings()
            {
                ApiKey = "56bc95025330b544721626eb473945e3",
                SearchBaseUrl = "https://api.themoviedb.org/3/search/movie",
                MovieBaseUrl = "https://api.themoviedb.org/3/movie",
                BaseUrl = "https://api.themoviedb.org/3",
                RestCountriesBaseUrl = "https://restcountries.com/v2/alpha",
            };

            _mongoDbRunner = MongoDbRunner.Start();
            var client = new MongoClient(_mongoDbRunner.ConnectionString);

            _context = new DBContextFixture(client, "IntegrationTest");
            _movieRepository = new MovieRepository(_context);
            _directorRepository = new DirectorRepository(_context);
            _logger = new Logger<MovieService>(new LoggerFactory());
            var mockFactory = new Mock<IHttpClientFactory>();
            var httpClient = new HttpClient();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var mockSettings = new Mock<IOptions<ApiSettings>>();
            mockSettings.Setup(ap => ap.Value).Returns(settings);
            _tmdbService = new TMDbService(mockFactory.Object, mockSettings.Object);
            _movieService = new MovieService(_movieRepository, _directorRepository, new ExcelFileParser(), _logger, _tmdbService);
            // Arrange

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
                Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America", Region = "Americas" } }
            };
            // Remplissage de la base de données avec des données de test...
            _context.Movies.InsertOne(_newMovie);


        }

        [Fact]
        public async Task GetByViewingYearsDate_ReturnsCorrectMovies()
        {
            // Arrange
            var expectedMovies = new List<MovieDAO> { _newMovie };

            // Act
            var actualMovies = await _movieService.GetByViewingYearsDate(2023);

            // Assert
            Assert.Equal(expectedMovies.First().Id, actualMovies.First().Id);

        }

        [Fact]
        public async Task GetByViewingYearsDate_ReturnsMightWatchCorrectMovies()
        {
            // Arrange
            var expectedMovies = new List<MovieDAO> { _newMovie };

            // Act
            var actualMovies = await _movieService.GetByViewingYearsDate(2023);

            // Assert
            Assert.Equal(expectedMovies.First().Id, actualMovies.First().Id);

        }

        [Fact]
        public async Task Update_Movie_Successfully_Updates_Movie_And_Director()
        {
            // Arrange
            var existingDirector = new Director { Id = 1, Name = "Existing Director" };
            _context.Directors.InsertOne(existingDirector);
            // Prepare the director with updated name
            var updatedDirector = new Director { Id = 1, Name = "Updated Director" };

            var movieToUpdate = new Movie
            {
                Uuid = _newMovie.Uuid,
                Id = _newMovie.Id,
                Directors = new List<Director> { updatedDirector },
                Title = "Film 2",
                OriginalTitle = "Film 2",
                Overview = _newMovie.OriginalTitle,
                ReleaseDate = _newMovie.ReleaseDate,
                PosterPath = _newMovie.PosterPath,
                BackdropPath = _newMovie.BackdropPath,
                Support = Support.Netflix,
                Genres = new List<Genre> { new Genre { Id = 17, Name = "SF" } },
                Countries = _newMovie.Countries
            };



            // Act
            var result = await _movieService.Update(movieToUpdate);

            // Retrieve the updated movie and director from the database
            var updatedMovieDao = _context.Movies.Find(m => m.Uuid == result.Uuid).FirstOrDefault();
            var updatedDirectorInDb = _context.Directors.Find(d => d.Id == updatedDirector.Id).FirstOrDefault();

            // Assert
            Assert.NotNull(updatedMovieDao);
            Assert.Equal(movieToUpdate.Id, result.Id);
            Assert.Equal(movieToUpdate.Title, result.Title);
            Assert.Equal(movieToUpdate.OriginalTitle, result.OriginalTitle);
            Assert.Equal(movieToUpdate.Support, result.Support);
            Assert.Equal(movieToUpdate.Genres.First().Id, result.Genres.First().Id);
            Assert.Equal(updatedDirector.Name, updatedDirectorInDb.Name);
        }

        [Fact]
        public async Task Delete_Movie_Successfully_Removes_Movie()
        {
            // Arrange
            var movieToDelete = new MovieDAO
            {
                Uuid = Guid.NewGuid(),
                Id = 5,
                DirectorsIds = new List<int> { 1, 2 },
                Title = "Film To Delete",
                OriginalTitle = "Film To Delete",
                Overview = "This film will be deleted",
                ReleaseDate = new DateTime(2023, 5, 5),
                PosterPath = "/path/to/poster.jpg",
                BackdropPath = "/path/to/backdrop.jpg",
                Support = Support.None,
                Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America" } }
            };
            _context.Movies.InsertOne(movieToDelete);

            // Act
            await _movieService.Delete(movieToDelete.Uuid);

            // Assert
            var deletedMovie = await _movieRepository.GetById(movieToDelete.Id);
            Assert.Null(deletedMovie);
        }

        [Fact]
        public async Task Upload_Files_Succefull()
        {
            // Arrange
            var fileName = "FilmsUpload.xlsx";
            var filePath = Path.Combine("Files", fileName);

            await using var stream = File.OpenRead(filePath);
            var file = new FormFile(stream, 0, stream.Length, null, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };

            // Act
            var message = await _movieService.Upload(file);

            // Assert
            Assert.NotEmpty(message);
        }


        public void Dispose()
        {
            // Destruction de l'environnement de test...
            _mongoDbRunner.Dispose();
        }
    }

}