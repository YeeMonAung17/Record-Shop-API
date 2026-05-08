using Record_Shop.Models;

namespace Record_Shop.Repositories
{
    public interface IAlbumRepository
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<Album?> GetAlbumByIdAsync(int id);
        Task<IEnumerable<Album>> GetAlbumsByArtistAsync(string artist);
        //Task<IEnumerable<Album>> GetAlbumsByYearAsync(int year);
        //Task<IEnumerable<Album>> GetAlbumsByGenreAsync(string genre);
        //Task<Album?> GetAlbumByTitleAsync(string title);
        Task<Album> AddAlbumAsync(Album album);
        Task<Album> UpdateAlbumAsync(Album album);
        Task<bool> DeleteAlbumAsync(int id);


       
    }
}
