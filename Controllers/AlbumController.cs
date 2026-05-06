using Microsoft.AspNetCore.Mvc;
using Record_Shop.Services;

namespace Record_Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAlbums()
        {
            var albums = await _albumService.GetAllAlbumsAsync();
            return Ok(albums.ToList());
        }
    }
}
