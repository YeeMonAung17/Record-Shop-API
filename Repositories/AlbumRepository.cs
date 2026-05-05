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
            using (_recordDbContext)
                return await _recordDbContext.Albums.ToListAsync();
        }

        public async Task<Album> GetAlbumByIdAsync(int id)
        {
            using (_recordDbContext)
                return await _recordDbContext.Albums.FindAsync(id);
        }

        public async Task AddAlbumAsync(Album album)
        {
            using (_recordDbContext)
            {
                await _recordDbContext.Albums.AddAsync(album);
                await _recordDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAlbumAsync(Album album)
        {
            using (_recordDbContext)
            {
                _recordDbContext.Albums.Update(album);
                await _recordDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAlbumAsync(int id)
        {
            using (_recordDbContext)
            {
                var album = await _recordDbContext.Albums.FindAsync(id);
                if (album != null)
                {
                    _recordDbContext.Albums.Remove(album);
                    await _recordDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
