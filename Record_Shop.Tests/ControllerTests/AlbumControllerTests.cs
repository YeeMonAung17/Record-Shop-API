using Microsoft.AspNetCore.Mvc;
using Moq;
using Record_Shop.Controllers;
using Record_Shop.Models;
using Record_Shop.Services;

namespace Record_Shop.Tests.ControllerTests
{
    public class AlbumControllerTests
    {
        private AlbumController _albumController;
        private Mock<IAlbumService> _albumServiceMoq;

        [SetUp]
        public void Setup()
        {
            _albumServiceMoq = new Mock<IAlbumService>();
            _albumController = new AlbumController(_albumServiceMoq.Object);
        }

        [Test]
        public async Task GetAllAlbumsAsync_ReturnsAllAlbums()
        {
            //Arrange
            var albums = new List<Album>
            {
                new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021 , Price = 12.99m, Stock = 10 },
                new Album { Id = 2, Title = "Album 2", Artist = "Artist 2",Genre = "Genre 2", Year = 2023, Price = 14.99m, Stock = 12 }
            };
            _albumServiceMoq.Setup(repo => repo.GetAllAlbumsAsync()).ReturnsAsync(albums);

            //Act
            var result = await _albumController.GetAllAlbums();
            var okResult = (OkObjectResult)result;
            //Assert
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var returnedAlbums = okResult.Value as List<Album>;
            Assert.That(returnedAlbums, Is.EquivalentTo(albums));
        }
    }
}
