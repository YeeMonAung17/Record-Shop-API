using Microsoft.EntityFrameworkCore;
using Record_Shop.Controllers;
using Record_Shop.Data;
using Record_Shop.Models;
using Record_Shop.Repositories;
using Record_Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Record_Shop.Tests.RepositoryTests
{
    public class AlbumRepositoryTests
    {
        private RecordDbContext _context;
        private AlbumRepository _albumRepository;


        [SetUp]
        public void Setup()
        {
            //fake in-memory database for testing
            var options = new DbContextOptionsBuilder<RecordDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;
            _context = new RecordDbContext(options);
            _albumRepository = new AlbumRepository(_context);
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAlbumsAsync_ReturnsAllAlbums()
        {
            //Arrange
            _context.Albums.Add(new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 });
            _context.Albums.Add(new Album { Id = 2, Title = "Album 2", Artist = "Artist 2", Genre = "Genre 2", Year = 2023, Price = 14, Stock = 12 });
            await _context.SaveChangesAsync();
            // Act
            var result = await _albumRepository.GetAllAlbumsAsync();
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Title, Is.EqualTo("Album 1"));
        }

        [Test]
        public async Task GetAllAlbumsAsync_ReturnsEmptyList_WhenNoAlbums()
        {
            // Act
            var result = await _albumRepository.GetAllAlbumsAsync();
            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAlbumByIdAsync_ReturnsAlbumId_WhenExists()
        {
            // Arrange
            var album = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 };
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
            // Act
            var result = await _albumRepository.GetAlbumByIdAsync(1);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Album 1"));
        }

        [Test]
        public async Task GetAlbumByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Act
            var result = await _albumRepository.GetAlbumByIdAsync(999);
            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-999)]
        public async Task GetAlbumByIdAsync_ReturnsNull_WhenInvalidId(int invalidId)
        {
            // Act
            var result = await _albumRepository.GetAlbumByIdAsync(invalidId);
            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddAlbumAsync_AddsAlbumToDatabase_AndReturnsWithId()
        {
            // Arrange
            var album = new Album { Title = "New Album", Artist = "New Artist", Genre = "New Genre", Year = 2024, Price = 15, Stock = 20 };
            // Act
            var result = await _albumRepository.AddAlbumAsync(album);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.GreaterThan(0));
            Assert.That(result.Title, Is.EqualTo("New Album"));

            // Verify it's actually in the database
            var albumInDb = await _context.Albums.FindAsync(result.Id);
            Assert.That(albumInDb, Is.Not.Null);
            Assert.That(albumInDb.Title, Is.EqualTo("New Album"));

        }

        [Test]
        public async Task AddAlbumAsync_AssignsUniqueIds_ToMultipleAlbums()
        {
            // Arrange
            var album1 = new Album { Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2024, Price = 15, Stock = 20 };
            var album2 = new Album { Title = "Album 2", Artist = "Artist 2", Genre = "Genre 2", Year = 2024, Price = 15, Stock = 20 };
            // Act
            var result1 = await _albumRepository.AddAlbumAsync(album1);
            var result2 = await _albumRepository.AddAlbumAsync(album2);
            // Assert
            Assert.That(result1.Id, Is.GreaterThan(0));
            Assert.That(result2.Id, Is.GreaterThan(0));
            Assert.That(result1.Id, Is.Not.EqualTo(result2.Id));
        }

        [Test]
        public async Task AddAlbumAsync_PersistsAlbumToDatabase()
        {
            // Arrange
            var album = new Album { Title = "Persistent Album", Artist = "Persistent Artist", Genre = "Persistent Genre", Year = 2024, Price = 15, Stock = 20 };
            // Act
            var result = await _albumRepository.AddAlbumAsync(album);
            // Assert
            var albums = await _context.Albums.ToListAsync();
            Assert.That(albums.Count, Is.EqualTo(1));
            Assert.That(albums.First().Title, Is.EqualTo("Persistent Album"));
        }
        [Test]
        public async Task UpdateAlbumAsync_ShouldModifyExistingRecord()
        {
            //Arrange
            var myAlbum = new Album { Title = "Original Album", Artist = "Original Artist", Genre = "Original Genre", Year = 2024, Price = 15, Stock = 20 };
            _context.Albums.Add(myAlbum);
            await _context.SaveChangesAsync();

            _context.Entry(myAlbum).State = EntityState.Detached;

            //Act
            var updatedAlbum = new Album { Id = myAlbum.Id, Title = "Updated Album", Artist = "Updated Artist", Genre = "Updated Genre", Year = 2025, Price = 19, Stock = 15 };
            await _albumRepository.UpdateAlbumAsync(updatedAlbum);

            //Assert
            var albumInDb = await _context.Albums.FindAsync(myAlbum.Id);
            Assert.That(albumInDb, Is.Not.Null);
            Assert.That(albumInDb.Title.Trim(), Is.EqualTo("Updated Album"));
            Assert.That(albumInDb.Artist.Trim(), Is.EqualTo("Updated Artist"));
            Assert.That(albumInDb.Genre.Trim(), Is.EqualTo("Updated Genre"));
            Assert.That(albumInDb.Year, Is.EqualTo(2025));
            Assert.That(albumInDb.Price, Is.EqualTo(19));
            Assert.That(albumInDb.Stock, Is.EqualTo(15));
        }

        [Test]
        public async Task UpdateAlbumAsync_ShouldReturnUpdatedAlbumObject()
        {
            //Arrange
            var myAlbum = new Album { Title = "Original Album", Artist = "Original Artist", Genre = "Original Genre", Year = 2024, Price = 15, Stock = 20 };
            _context.Albums.Add(myAlbum);
            await _context.SaveChangesAsync();
            _context.Entry(myAlbum).State = EntityState.Detached;
            //Act
            var updatedAlbum = new Album { Id = myAlbum.Id, Title = "Updated Album", Artist = "Updated Artist", Genre = "Updated Genre", Year = 2025, Price = 19, Stock = 15 };
            var result = await _albumRepository.UpdateAlbumAsync(updatedAlbum);
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(myAlbum.Id));
            Assert.That(result.Title, Is.EqualTo("Updated Album"));
            Assert.That(result.Artist, Is.EqualTo("Updated Artist"));
            Assert.That(result.Genre, Is.EqualTo("Updated Genre"));
            Assert.That(result.Year, Is.EqualTo(2025));
            Assert.That(result.Price, Is.EqualTo(19));
            Assert.That(result.Stock, Is.EqualTo(15));
        }

        [Test]
        public async Task DeleteAlbumAsync_ShouldRemoveRecordFromDb()
        {
            //Arrange
            var album = new Album { Title = "Album to Delete", Artist = "Artist to Delete", Genre = "Genre to Delete", Year = 2024, Price = 15, Stock = 20 };
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            int albumId = album.Id;
            //Act
            await _albumRepository.DeleteAlbumAsync(albumId);
            //Assert
            var albumInDb = await _context.Albums.FindAsync(albumId);
            Assert.That(albumInDb, Is.Null);

        }
        [Test]
        public async Task GetAlbumsByArtistAsync_WhenArtistExists_ReturnsOnlyMatchingAlbums()
        {
            //Arrange
            var artistToFind = "Artist A";
            _context.Albums.Add(new Album { Title = "Album 1", Artist = artistToFind, Genre = "Genre 1", Year = 2021, Price = 12, Stock = 10 });
            _context.Albums.Add(new Album { Title = "Album 2", Artist = "Artist B", Genre = "Genre 2", Year = 2023, Price = 14, Stock = 12 });
            _context.Albums.Add(new Album { Title = "Album 3", Artist = artistToFind, Genre = "Genre 3", Year = 2022, Price = 13, Stock = 8 });
            await _context.SaveChangesAsync();
            //Act
            var result = await _albumRepository.GetAlbumsByArtistAsync(artistToFind);
            //Assert
            var resultList = result.ToList();
            Assert.That(resultList.Count, Is.EqualTo(2));
            Assert.That(resultList.All(a => a.Artist == artistToFind), Is.True);
            Assert.That(resultList.Any(a => a.Title == "Album 1"), Is.True);
            Assert.That(resultList.Any(a=> a.Title == "Album 3"), Is.True);
        }

        [Test]
        public async Task GetAlbumsByArtistAsync_WhenArtistDoesNotExist_ReturnsEmptyCollection()
        {
            // Act
            var result = await _albumRepository.GetAlbumsByArtistAsync("NonExistentArtist");

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAlbumsByYearAsync_ReturnsMatchingAlbums_WhenYearExists()
        {
            //Arrange
            var yearToFind = 2022;
            _context.Albums.Add(new Album { Title = "Album 1", Artist = "Artist A", Genre = "Genre 1", Year = yearToFind, Price = 12, Stock = 10 });
            _context.Albums.Add(new Album { Title = "Album 2", Artist = "Artist B", Genre = "Genre 2", Year = 2023, Price = 14, Stock = 12 });
            _context.Albums.Add(new Album { Title = "Album 3", Artist = "Artist C", Genre = "Genre 3", Year = yearToFind, Price = 13, Stock = 8 });
            await _context.SaveChangesAsync();
            //Act
            var result = await _albumRepository.GetAlbumsByYearAsync(yearToFind);
            //Assert
            var resultList = result.ToList();
            Assert.That(resultList.Count, Is.EqualTo(2));
            Assert.That(resultList.All(a => a.Year == yearToFind), Is.True);
            Assert.That(resultList.Any(a => a.Title == "Album 1"), Is.True);
            Assert.That(resultList.Any(a => a.Title == "Album 3"), Is.True);
        }

        [Test]
        public async Task GetAlbumsByYearAsync_WhenYearDoesNotExist_ReturnsEmptyCollection()
        {
            // Act
            var result = await _albumRepository.GetAlbumsByYearAsync(9999);

            // Assert
            Assert.That(result, Is.Empty);
        }

    }
}
