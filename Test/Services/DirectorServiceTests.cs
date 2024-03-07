using Models;
using Moq;
using Repository.Interface;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Services
{
    public  class DirectorServiceTests
    {
        [Fact]
        public async Task Create_ReturnsSavedDirector()
        {
            // Arrange
            var director = new Director { Name = "Test Director" };
            var directorRepositoryMock = new Mock<IDirectorRepository>();
            directorRepositoryMock.Setup(repo => repo.Save(director)).ReturnsAsync(director);
            var directorService = new DirectorService(directorRepositoryMock.Object);

            // Act
            var createdDirector = await directorService.Create(director);

            // Assert
            Assert.NotNull(createdDirector);
            Assert.Equal(director.Name, createdDirector.Name);
            directorRepositoryMock.Verify(repo => repo.Save(director), Times.Once);
        }

        [Fact]
        public async Task GetByName_ReturnsDirector()
        {
            // Arrange
            var name = "Test Director";
            var expectedDirector = new Director { Name = name };
            var directorRepositoryMock = new Mock<IDirectorRepository>();
            directorRepositoryMock.Setup(repo => repo.GetByName(name)).ReturnsAsync(expectedDirector);
            var directorService = new DirectorService(directorRepositoryMock.Object);

            // Act
            var actualDirector = await directorService.GetByName(name);

            // Assert
            Assert.NotNull(actualDirector);
            Assert.Equal(expectedDirector.Name, actualDirector.Name);
            directorRepositoryMock.Verify(repo => repo.GetByName(name), Times.Once);
        }


    }
}
