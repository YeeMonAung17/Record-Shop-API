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
                new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021 , Price = 12, Stock = 10 },
                new Album { Id = 2, Title = "Album 2", Artist = "Artist 2",Genre = "Genre 2", Year = 2023, Price = 14, Stock = 12 }
            };
            _albumRepositoryMoq.Setup(repo => repo.GetAllAlbumsAsync()).ReturnsAsync(albums);

            //Act
            var result = await _albumService.GetAllAlbumsAsync();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
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

        [Test]

        public async Task GetAlbumByIdAsync_ReturnsAlbumId_WhenExists()
        {
            //Arrange
            var album = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumRepositoryMoq.Setup(repo => repo.GetAlbumByIdAsync(1)).ReturnsAsync(album);
            //Act
            var result = await _albumService.GetAlbumByIdAsync(1);
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Album 1"));
            _albumRepositoryMoq.Verify(repo => repo.GetAlbumByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task GetAlbumByIdAsync_ReturnsNull_WhenNotExists()
        {
            //Arrange
            _albumRepositoryMoq.Setup(repo => repo.GetAlbumByIdAsync(1)).ReturnsAsync((Album?)null);
            //Act
            var result = await _albumService.GetAlbumByIdAsync(1);
            //Assert
            Assert.That(result, Is.Null);
            _albumRepositoryMoq.Verify(repo => repo.GetAlbumByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task AddAlbumAsync_ReturnsAddedAlbum()
        {
            //Arrange
            var album = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };

            var addedAlbum = new Album
            {
                Id = 1,
                Title = "Album 1",
                Artist = "Artist 1",
                Genre = "Genre 1",
                Year = 2021,
                Price = 13,
                Stock = 10
            };


            _albumRepositoryMoq.Setup(repo => repo.AddAlbumAsync(It.IsAny<Album>())).ReturnsAsync(addedAlbum);
            //Act
            var result = await _albumService.AddAlbumAsync(album);
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo("Album 1"));
            _albumRepositoryMoq.Verify(repo => repo.AddAlbumAsync(It.IsAny<Album>()), Times.Once);
        }

        [Test]
        public async Task UpdateAlbum_WhenAlbumExists_ReturnsUpdatedAlbum()
        {
            //Arrange
            int albumId = 1;
            var album = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            var updatedAlbum = new Album { Id = 1, Title = "Updated Album", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumRepositoryMoq.Setup(repo => repo.GetAlbumByIdAsync(albumId)).ReturnsAsync(album);
            _albumRepositoryMoq.Setup(repo => repo.UpdateAlbumAsync(It.IsAny<Album>())).ReturnsAsync(updatedAlbum);
            //Act
            var result = await _albumService.UpdateAlbumAsync(albumId, updatedAlbum);
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Updated Album"));
            _albumRepositoryMoq.Verify(repo => repo.UpdateAlbumAsync(It.IsAny<Album>()), Times.Once);
        }

        [Test]
        public async Task UpdateAlbum_WhenAlbumDoesNotExist_ReturnsNull()
        {
            //Arrange
            int id = 999; // Non-existing ID
            var album = new Album { Id = id, Title = "Non-existing Album", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumRepositoryMoq.Setup(repo => repo.GetAlbumByIdAsync(id)).ReturnsAsync((Album?)null);
            //Act
            var result = await _albumService.UpdateAlbumAsync(id, album);
            //Assert
            Assert.That(result, Is.Null);
            _albumRepositoryMoq.Verify(repo => repo.UpdateAlbumAsync(It.IsAny<Album>()), Times.Never);
        }

        [Test]
        public async Task DeleteAlbumAsync_ReturnsTrue_WhenAlbumExists()
        {
            //Arrange
            int albumId = 1;
            var album = new Album { Id = albumId, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumRepositoryMoq.Setup(repo => repo.GetAlbumByIdAsync(albumId)).ReturnsAsync(album);
            _albumRepositoryMoq.Setup(repo => repo.DeleteAlbumAsync(albumId)).ReturnsAsync(true);
            //Act
            var result = await _albumService.DeleteAlbumAsync(albumId);
            //Assert
            Assert.That(result, Is.True);
            _albumRepositoryMoq.Verify(repo => repo.DeleteAlbumAsync(albumId), Times.Once);
        }
        [Test]
        public async Task GetAlbumsByArtistAsync_ShouldReturnFilteredAlbums()
        {
            //Arrange
            var artistName = "Taylor Swift";
            var fakeAlbums = new List<Album>
            {
                new Album { Id = 1, Title = "Album 1", Artist = "Taylor Swift", Genre = "Pop", Year = 2020, Price = 15, Stock = 5 },
                new Album { Id = 2, Title = "Album 2", Artist = "Taylor Swift", Genre = "Pop", Year = 2021, Price = 16, Stock = 3 }
            };
            _albumRepositoryMoq.Setup(repo => repo.GetAlbumsByArtistAsync(artistName)).ReturnsAsync(fakeAlbums);

            //Act
            var result = await _albumService.GetAlbumsByArtistAsync(artistName);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            _albumRepositoryMoq.Verify(repo => repo.GetAlbumsByArtistAsync(artistName), Times.Once);

        }

        [Test]
        public async Task GetAlbumsByArtistAsync_ShouldReturnEmptyList_WhenNoAlbumsFound()
        {
            //Arrange
            var artistName = "NonExistingArtist";
            _albumRepositoryMoq.Setup(repo => repo.GetAlbumsByArtistAsync(artistName)).ReturnsAsync(new List<Album>());
            //Act
            var result = await _albumService.GetAlbumsByArtistAsync(artistName);
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
            _albumRepositoryMoq.Verify(repo => repo.GetAlbumsByArtistAsync(artistName), Times.Once);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task GetAlbumsByArtistAsync_ShouldThrowException_WhenInvalidArtistName(string invalidArtistName)
        {
            //Arrange
            var service = new AlbumService(_albumRepositoryMoq.Object);
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAlbumsByArtistAsync(invalidArtistName));

            //Assert
            _albumRepositoryMoq.Verify(repo => repo.UpdateAlbumAsync(It.IsAny<Album>()), Times.Never);

        }
    }

    }

