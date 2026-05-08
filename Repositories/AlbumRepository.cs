using Microsoft.EntityFrameworkCore;
using Record_Shop.Data;
using Record_Shop.Models;

namespace Record_Shop.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly RecordDbContext _recordDbContext;

        public AlbumRepository(RecordDbContext recordDbContext)
        {
            _recordDbContext = recordDbContext;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {

            return await _recordDbContext.Albums.ToListAsync();
        }

        public async Task<Album?> GetAlbumByIdAsync(int id)
        {

            var album = await _recordDbContext.Albums.FirstOrDefaultAsync(a => a.Id == id);
            return album;
        }

        public async Task<Album> AddAlbumAsync(Album album)
        {

            await _recordDbContext.Albums.AddAsync(album);
            await _recordDbContext.SaveChangesAsync();
            return album;

        }

        public async Task<Album> UpdateAlbumAsync(Album album)
        {
            _recordDbContext.Albums.Update(album);
            await _recordDbContext.SaveChangesAsync();
            return album;

        }

        public async Task<bool> DeleteAlbumAsync(int id)
        {

            var album = await _recordDbContext.Albums.FindAsync(id);
            if (album == null)
            {
                return false;
            }

            _recordDbContext.Albums.Remove(album);
            await _recordDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<Album>> GetAlbumsByArtistAsync(string artist)
        {
            return await _recordDbContext.Albums.Where(a => a.Artist == artist).ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByYearAsync(int year)
        {
            return await _recordDbContext.Albums.Where(a => a.Year == year).ToListAsync();
        }
    }
}
