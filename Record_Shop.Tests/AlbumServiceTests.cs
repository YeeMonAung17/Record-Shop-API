using Moq;
using Record_Shop.Models;
using Record_Shop.Repositories;
using Record_Shop.Services;

namespace Record_Shop.Tests.ServiceTests
{
    [TestFixture]
    public class AlbumServiceTests
    {
        private Mock<IAlbumRepository> _albumRepositoryMoq;
        private AlbumService _albumService;

        [SetUp]
        public void Setup()
        {
            _albumRepositoryMoq = new Mock<IAlbumRepository>();
            _albumService = new AlbumService(_albumRepositoryMoq.Object);
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
            _albumRepositoryMoq.Setup(repo => repo.GetAllAlbumsAsync()).ReturnsAsync(albums);

            //Act
            var result = await _albumService.GetAllAlbumsAsync();

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Title, Is.EqualTo("Album 1"));
            _albumRepositoryMoq.Verify(repo => repo.GetAllAlbumsAsync(), Times.Once);


        }

        [Test]
        public async Task GetAllAlbumsAsync_ReturnsEmptyList_WhenNoAlbums()
        {
            //Arrange
            _albumRepositoryMoq.Setup(repo => repo.GetAllAlbumsAsync()).ReturnsAsync(new List<Album>());
            //Act
            var result = await _albumService.GetAllAlbumsAsync();
            //Assert
            Assert.That(result, Is.Empty);
            _albumRepositoryMoq.Verify(repo => repo.GetAllAlbumsAsync(), Times.Once);
        }
    }
}
