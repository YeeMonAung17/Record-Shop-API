using Record_Shop.Models;

namespace Record_Shop.Services
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<Album?> GetAlbumByIdAsync(int id);
        Task<Album> AddAlbumAsync(Album album);
        Task<Album?> UpdateAlbumAsync(int id, Album album);
        Task<bool> DeleteAlbumAsync(int id);
    }
}
