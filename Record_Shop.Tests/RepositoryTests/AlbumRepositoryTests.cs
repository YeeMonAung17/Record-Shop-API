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
            _context.Albums.Add(new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12.99m, Stock = 10 });
            _context.Albums.Add(new Album { Id = 2, Title = "Album 2", Artist = "Artist 2", Genre = "Genre 2", Year = 2023, Price = 14.99m, Stock = 12 });
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
            var album = new Album { Id = 1, Title = "Album 1", Artist = "Artist 1", Genre = "Genre 1", Year = 2021, Price = 12.99m, Stock = 10 };
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
    }
}
