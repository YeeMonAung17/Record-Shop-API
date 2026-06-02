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
                new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021 , Price = 12, Stock = 10 },
                new Album { Id = 2, Title = "Album 2", Artist = "Artist 2",Genre = "Genre 2", Year = 2023, Price = 14, Stock = 12 }
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

        [Test]
        public async Task GetAlbumByIdAsync_ReturnsAlbum_WhenExists()
        {
            //Arrange
            var album = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumServiceMoq.Setup(repo => repo.GetAlbumByIdAsync(1)).ReturnsAsync(album);
            //Act
            var result = await _albumController.GetAlbumById(1);
            var okResult = (OkObjectResult)result;
            //Assert
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var returnedAlbum = okResult.Value as Album;
            Assert.That(returnedAlbum, Is.EqualTo(album));
        }

        [Test]

        public async Task GetAlbumByIdAsync_ReturnsNotFound_WhenDoesNotExist()
        {
            //Arrange
            _albumServiceMoq.Setup(repo => repo.GetAlbumByIdAsync(1)).ReturnsAsync((Album?)null);
            //Act
            var result = await _albumController.GetAlbumById(1);
            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task AddAlbum_ReturnsCreatedAtAction_WhenAlbumIsValid()
        {
            //Arrange
            var album = new Album { Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            var createdAlbum = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumServiceMoq.Setup(repo => repo.AddAlbumAsync(It.IsAny<Album>())).ReturnsAsync(createdAlbum);
            //Act
            var result = await _albumController.AddAlbum(album);
            var createdAtActionResult = (CreatedAtActionResult)result;
            //Assert
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult.StatusCode, Is.EqualTo(201));

            var returnedAlbum = createdAtActionResult.Value as Album;
            Assert.That(returnedAlbum, Is.Not.Null);
            Assert.That(returnedAlbum.Id, Is.EqualTo(createdAlbum.Id));
            Assert.That(returnedAlbum.Title, Is.EqualTo(createdAlbum.Title));
        }
        [Test]
        public async Task AddAlbum_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            //Arrange
            var album = new Album { Title = "Test", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumController.ModelState.AddModelError("Title", "The Title field is required.");

            //Act
            var result = await _albumController.AddAlbum(album);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }
        [Test]
        public async Task UpdateAlbum_Returns201Created_WhenUpdateIsSuccessful()
        {
            //Arrange
            int albumId = 1;
            var updatedAlbum = new Album { Id = albumId, Title = "Updated Album", Artist = "Updated Artist", Genre = "Updated Genre", Year = 2022, Price = 15, Stock = 5 };
            _albumServiceMoq.Setup(repo => repo.UpdateAlbumAsync(albumId, It.IsAny<Album>())).ReturnsAsync(updatedAlbum);
            //Act
            var result = await _albumController.UpdateAlbum(albumId, updatedAlbum);
            //Assert

            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var actionResult = result as ObjectResult;
            Assert.That(actionResult.StatusCode, Is.EqualTo(200));

            var actualAlbum = actionResult.Value as Album;
            Assert.That(actualAlbum.Title, Is.EqualTo(updatedAlbum.Title));


        }
        [Test]
        public async Task UpdateAlbum_Returns400BadRequest_WhenModelStateIsInvalid()
        {
            //Arrange
            int albumId = 1;
            var updatedAlbum = new Album { Id = albumId, Title = "Updated Album", Artist = "Updated Artist", Genre = "Updated Genre", Year = 2022, Price = 15, Stock = 5 };
            _albumController.ModelState.AddModelError("Title", "The Title field is required.");

            //Act
            var result = await _albumController.UpdateAlbum(albumId, updatedAlbum);

            //Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

        }
        [Test]
        public async Task UpdateAlbum_Returns404NotFound_WhenAlbumDoesNotExist()
        {
            //Arrange
            int albumId = 1;
            var updatedAlbum = new Album { Id = albumId, Title = "Updated Album", Artist = "Updated Artist", Genre = "Updated Genre", Year = 2022, Price = 15, Stock = 5 };
            _albumServiceMoq.Setup(repo => repo.UpdateAlbumAsync(albumId, It.IsAny<Album>())).ReturnsAsync((Album)null);

            //Act
            var result = await _albumController.UpdateAlbum(albumId, updatedAlbum);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task DeleteAlbum_Returns204NoContent_WhenSuccessful()
        {
            //Arrange
            int albumId = 1;
            _albumServiceMoq.Setup(repo => repo.DeleteAlbumAsync(albumId)).ReturnsAsync(true);

            //Act
            var result = await _albumController.DeleteAlbum(albumId);

            //Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            var noContentResult = result as NoContentResult;
            Assert.That(noContentResult, Is.Not.Null);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task DeleteAlbum_Returns404NotFound_WhenAlbumMissing()
        {
            //Arrange
            _albumServiceMoq.Setup(repo => repo.DeleteAlbumAsync(99)).ReturnsAsync(false);

            //Act
            var result = await _albumController.DeleteAlbum(99);

            //Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task GetAllAlbumsByArtist_ReturnAlbums_WhenArtistExists()
        {
            //Arrange
            var album = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumServiceMoq.Setup(repo => repo.GetAlbumsByArtistAsync("Artist 1")).ReturnsAsync(new List<Album> { album });

            //Act
            var result = await _albumController.GetAlbumsByArtist("Artist 1");

            //Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var albums = okResult.Value as List<Album>;
            Assert.That(albums, Is.Not.Null);
            Assert.That(albums.Count, Is.EqualTo(1));
            Assert.That(albums[0].Title, Is.EqualTo("Album 1"));
        }

        [Test]
        public async Task GetAllAlbumsByArtist_ReturnsEmptyList_WhenArtistDoesNotExist()
        {
            //Arrange
            _albumServiceMoq.Setup(repo => repo.GetAlbumsByArtistAsync("NonExistent Artist")).ReturnsAsync(new List<Album>());
            //Act
            var result = await _albumController.GetAlbumsByArtist("NonExistent Artist");
            //Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var albums = okResult.Value as List<Album>;
            Assert.That(albums, Is.Not.Null);
            Assert.That(albums.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetByArtist_ReturnsBadRequest_WhenArtistNameIsEmpty()
        {
            // Act
            var result = await _albumController.GetAlbumsByArtist("");

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetAlbumsByYear_Returns200OK_WhenYearIsValid()
        {
            // Arrange
            var album = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumServiceMoq.Setup(repo => repo.GetAlbumsByYearAsync(2021)).ReturnsAsync(new List<Album> { album });
            // Act
            var result = await _albumController.GetAlbumsByYear(2021);
            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var albums = okResult.Value as List<Album>;
            Assert.That(albums, Is.Not.Null);
            Assert.That(albums.Count, Is.EqualTo(1));
            Assert.That(albums[0].Title, Is.EqualTo("Album 1"));
        }

        [Test]
        public async Task GetAlbumsByYears_ReturnsEmptyList_WhenNoAlbumsExistsInGivenYear()
        {
            // Arrange
            _albumServiceMoq.Setup(repo => repo.GetAlbumsByYearAsync(1990)).ReturnsAsync(new List<Album>());
            // Act
            var result = await _albumController.GetAlbumsByYear(1990);
            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var albums = okResult.Value as List<Album>;
            Assert.That(albums, Is.Not.Null);
            Assert.That(albums.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAlbumsByGenre_Returns200OK_WhenGenreIsValid()
        {
            // Arrange
            var album = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumServiceMoq.Setup(repo => repo.GetAlbumsByGenreAsync("Genre 1")).ReturnsAsync(new List<Album> { album });
            // Act
            var result = await _albumController.GetAlbumsByGenre("Genre 1");
            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var albums = okResult.Value as List<Album>;
            Assert.That(albums, Is.Not.Null);
            Assert.That(albums.Count, Is.EqualTo(1));
            Assert.That(albums[0].Title, Is.EqualTo("Album 1"));
        }

        [Test]
        public async Task GetAlbumsByGenre_ReturnsOkWithEmptyList_WhenNoMatches()
        {
            // Arrange
            _albumServiceMoq.Setup(repo => repo.GetAlbumsByGenreAsync("NonExistent Genre")).ReturnsAsync(new List<Album>());
            // Act
            var result = await _albumController.GetAlbumsByGenre("NonExistent Genre");
            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var albums = okResult.Value as List<Album>;
            Assert.That(albums, Is.Not.Null);
            Assert.That(albums.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAlbumsByGenre_CallsServiceWithCorrectGenre()

        {
            // Arrange
            string genre = "Rock";
            _albumServiceMoq.Setup(repo => repo.GetAlbumsByGenreAsync(genre)).ReturnsAsync(new List<Album>());
            // Act
            var result = await _albumController.GetAlbumsByGenre(genre);
            // Assert
            _albumServiceMoq.Verify(repo => repo.GetAlbumsByGenreAsync(genre), Times.Once);
        }
        [Test]
        public async Task GetAlbumByTitle_Returns200Ok_WhenAlbumExists()
        {
            // Arrange
            var album = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _albumServiceMoq.Setup(repo => repo.GetAlbumByTitleAsync("Album 1")).ReturnsAsync(album);
            // Act
            var result = await _albumController.GetAlbumByTitle("Album 1");
            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var returnedAlbum = okResult.Value as Album;
            Assert.That(returnedAlbum, Is.Not.Null);
            Assert.That(returnedAlbum.Title, Is.EqualTo("Album 1"));
        }
        [Test]
        public async Task GetAlbumByTitle_Returns404NotFound_WhenAlbumDoesNotExist()
        {
            // Arrange
            _albumServiceMoq.Setup(repo => repo.GetAlbumByTitleAsync("NonExistent Album")).ReturnsAsync((Album?)null);
            // Act
            var result = await _albumController.GetAlbumByTitle("NonExistent Album");
            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task GetAlbumByTitle_ReturnsBadRequest_WhenTitleIsInvalid(string? invalidTitle)
        {
            // Act
            var result = await _albumController.GetAlbumByTitle(invalidTitle);
            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

            _albumServiceMoq.Verify(repo => repo.GetAlbumByTitleAsync(It.IsAny<string>()), Times.Never);

        }
    }
}
