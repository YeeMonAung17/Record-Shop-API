using Record_Shop.Models;
using Record_Shop.Repositories;

namespace Record_Shop.Services
{
    public class AlbumService : IAlbumService
    { 
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
           return await _albumRepository.GetAllAlbumsAsync();
        }

        public async Task<Album?> GetAlbumByIdAsync(int id)
        {
            return await _albumRepository.GetAlbumByIdAsync(id);
        }

        public async Task<Album> AddAlbumAsync(Album album)
        {
           return await _albumRepository.AddAlbumAsync(album);
        }


        public async Task<Album?> UpdateAlbumAsync(int id, Album album)
        {
            
            var existingAlbum = await _albumRepository.GetAlbumByIdAsync(id);
            if (existingAlbum == null)
            {
                return null;
            }           
           
            existingAlbum.Title = album.Title;
            existingAlbum.Artist = album.Artist;
            existingAlbum.Genre = album.Genre;
            existingAlbum.Year = album.Year;
            existingAlbum.Price = album.Price;
            existingAlbum.Stock = album.Stock;
            await _albumRepository.UpdateAlbumAsync(existingAlbum);
            return existingAlbum;
        }


        public async Task<bool> DeleteAlbumAsync(int id)
        {
           
            return await _albumRepository.DeleteAlbumAsync(id);
        }
    }
}
