using System;
using System.Threading.Tasks;
using Film.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DAO;
using Models.ViewModel;
using Moq;
using Moq.Language.Flow;
using Services.Exceptions;
using Services.Interfaces;
using Xunit;

namespace UnitTest.Controller
{

    public class MoviesControllerTests
    {
        private Mock<IMovieService> _movieServiceMock;
        private MoviesController _controller;
        private Mock<ILogger<MoviesController>> _logger;
        public MoviesControllerTests()
        {
            _movieServiceMock = new Mock<IMovieService>();
            _logger = new Mock<ILogger<MoviesController>>();
            _controller = new MoviesController(_movieServiceMock.Object, _logger.Object);
        }
        [Fact]
        public async Task Post_Returns_Ok_With_Created_Movie()
        {
            var movie = new Movie();
            // Act
            var result = await _controller.Post(movie);

            // Assert
            Assert.IsType<OkResult>(result);

        }

        [Fact]
        public async Task Post_Returns_Conflict_When_Conflict_Exception_Is_Thrown()
        {
            // Arrange

            var movie = new Movie { Title = "Test Movie" };
            _movieServiceMock.Setup(s => s.Create(movie)).ThrowsAsync(new ConflictException("Movie already exists"));

            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await _controller.Post(movie) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
            Assert.Equal("Movie already exists", result.Value);
        }

        [Fact]
        public async Task Post_Returns_InternalServerError_When_Generic_Exception_Is_Thrown()
        {
            // Arrange
            var movie = new Movie { Title = "Test Movie" };
            _movieServiceMock.Setup(s => s.Create(movie)).ThrowsAsync(new Exception("Something went wrong"));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await _controller.Post(movie) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
            Assert.Equal("Something went wrong", result.Value);
        }

        [Fact]
        public async Task GetByViewingYearsDate_ReturnsExpectedMovies()
        {
            // Arrange
            var year = 2023;
            var expectedMovies = new List<Movie>
            {
                new Movie { Uuid = Guid.NewGuid(), Title = "Movie 1" },
                new Movie { Uuid = Guid.NewGuid(), Title = "Movie 2" }
            };

            _movieServiceMock.Setup(s => s.GetByViewingYearsDate(year)).ReturnsAsync(expectedMovies);

            // Act
            var actionResult = await _controller.GetByViewingYearsDate(year);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualMovies = Assert.IsType<List<Movie>>(okResult.Value);

            Assert.Equal(expectedMovies.Count, actualMovies.Count);

            for (int i = 0; i < expectedMovies.Count; i++)
            {
                Assert.Equal(expectedMovies[i].Uuid, actualMovies[i].Uuid);
                Assert.Equal(expectedMovies[i].Title, actualMovies[i].Title);
            }

            _movieServiceMock.Verify(s => s.GetByViewingYearsDate(year), Times.Once);
        }
    }
}



