using Models.ViewModel;
using Models;
using Moq;
using Repository.Interface;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DAO;
using Services.Interfaces;
using Services.Helpers;
using Microsoft.Extensions.Logging;

namespace UnitTest.Services
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IDirectorRepository> _directorRepositoryMock;
        private readonly MovieService _movieService;
        private readonly ILogger<MovieService> _loggerMock;
        private readonly TMDbService _tmdbServiceMock;
        private readonly ExcelFileParser _excelFileParserMock;
        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _directorRepositoryMock = new Mock<IDirectorRepository>();
   
            _excelFileParserMock = new Mock<ExcelFileParser>().Object;
            _loggerMock = new Mock<ILogger<MovieService>>().Object;
            _tmdbServiceMock = new Mock<TMDbService>().Object;
            _movieService = new MovieService(_movieRepositoryMock.Object, _directorRepositoryMock.Object, _excelFileParserMock, _loggerMock, _tmdbServiceMock);
        }

        [Fact]
        public async Task Create_WithNewMovie_CreatesMovieAndDirectorsIfTheyDoNotExist()
        {
            // Arrange
            MovieDAO? existingMovie = null;
            Director? existingDirector = null;
            _movieRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(existingMovie);
            _directorRepositoryMock.Setup(m => m.GetByName(It.IsAny<string>())).ReturnsAsync(existingDirector);
            _directorRepositoryMock.Setup(m => m.Save(It.IsAny<Director>())).ReturnsAsync(new Director());


            var movie = new Movie
            {
                Id = 1,
                Title = "Test Movie",
                Directors = new List<Director>
                {
                    new Director { Id = 1, Name = "Director 1" },
                    new Director { Id = 2, Name = "Director 2" }
                }
            };
            var movieDAO = new MovieDAO
            {
                Id = 1,
                Title = "Test Movie",
                DirectorsIds = new List<int>
                {
                   1,
                   2
                }

            };
            _movieRepositoryMock.Setup(m => m.Create(It.IsAny<MovieDAO>())).ReturnsAsync(movieDAO);
            // Act
            await _movieService.Create(movie);

            // Assert

        }

        [Fact]
        public async Task GetByViewingYearsDate_ShouldCallRepositoryMethodAndReturnItsResult()
        {
            // Arrange
            int yearDate = 2023;
            var expectedMovies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Test Movie 1" },
            new Movie { Id = 2, Title = "Test Movie 2" }
        };
            _movieRepositoryMock.Setup(repo => repo.GetByViewingYearsDate(yearDate)).ReturnsAsync(expectedMovies);

            // Act
            var result = await _movieService.GetByViewingYearsDate(yearDate);

            // Assert
            _movieRepositoryMock.Verify(repo => repo.GetByViewingYearsDate(yearDate), Times.Once);
            Assert.Equal(expectedMovies, result);
        }
        [Fact]
        public async Task Get_ReturnsMovie()
        {
            // Arrange
            Guid movieId = Guid.NewGuid();
            Movie expectedMovie = new Movie
            {
                Uuid = movieId,
                Title = "Sample Movie",
                // Ajoutez d'autres propriétés du film si nécessaire
            };

            // Mock du repository pour simuler le comportement de GetById()

            _movieRepositoryMock
                .Setup(repo => repo.GetById(movieId))
                .ReturnsAsync(expectedMovie);

           

            // Act
             var result = await _movieService.Get(movieId);

            //// Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMovie.Uuid, result.Uuid);
            Assert.Equal(expectedMovie.Title, result.Title);
            //Effectuez d'autres assertions sur les propriétés du film si nécessaire
        }

        [Fact]
        public async Task Update_Movie_Successfully_Updates_Movie_And_Director()
        {
            // Arrange
            var director = new Director { Id = 1, Name = "Director Name" };
            var movie = new Movie
            {
                Uuid = Guid.NewGuid(),
                Title = "Movie Title",
                Directors = new List<Director> { director }
            };
            var movieDao = movie.Map<Movie, MovieDAO>(); // Assuming you have a Map extension method
            movieDao.Title = "newTitle";
            _directorRepositoryMock.Setup(x => x.GetByName(It.IsAny<string>())).ReturnsAsync(director);
            _directorRepositoryMock.Setup(x => x.Update(It.IsAny<Director>())).ReturnsAsync((Director d) => d);
            _movieRepositoryMock.Setup(x => x.Update(It.IsAny<MovieDAO>())).ReturnsAsync(movieDao);

            // Act
            var result = await _movieService.Update(movie);

            // Assert
            Assert.Equal(movie.Id, result.Id);
            _directorRepositoryMock.Verify(x => x.GetByName(It.IsAny<string>()), Times.Exactly(movie.Directors.Count));
            _movieRepositoryMock.Verify(x => x.Update(It.IsAny<MovieDAO>()), Times.Once());
        }

        [Fact]
        public async Task Delete_Movie_Successfully_Removes_Movie()
        {
            // Arrange
            Guid movieIdToDelete = Guid.NewGuid();

            Movie movieToDelete = new Movie
            {
                Uuid = movieIdToDelete,
                Title = "Movie To Delete",
                // Add other properties as required
            };

            _movieRepositoryMock
                .Setup(repo => repo.GetById(movieIdToDelete))
                .ReturnsAsync(movieToDelete);

            _movieRepositoryMock
                .Setup(repo => repo.Delete(movieIdToDelete))
                .Returns(Task.CompletedTask);

            // Act
            await _movieService.Delete(movieIdToDelete);

            // Assert
            _movieRepositoryMock.Verify(repo => repo.Delete(movieIdToDelete), Times.Once);
        }

    }
}


