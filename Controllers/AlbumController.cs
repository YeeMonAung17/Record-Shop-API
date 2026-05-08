using Microsoft.AspNetCore.Mvc;
using Record_Shop.Models;
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

        [HttpGet("{id}")]

        public async Task<IActionResult> GetAlbumById(int id)
        {
            var album = await _albumService.GetAlbumByIdAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);
        }

        [HttpGet("artist/{artist}")]
        public async Task<IActionResult> GetAlbumsByArtist(string artist)
        {
            var albums = await _albumService.GetAlbumsByArtistAsync(artist);
            if (string.IsNullOrWhiteSpace(artist))
            {
                return BadRequest("Artist name is required."); 
            }
            return Ok(albums.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddAlbum([FromBody] Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAlbum = await _albumService.AddAlbumAsync(album);
            return CreatedAtAction(nameof(GetAlbumById), new { id = createdAlbum.Id }, createdAlbum);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlbum(int id, [FromBody] Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedAlbum = await _albumService.UpdateAlbumAsync(id, album);
            if (updatedAlbum == null)
            {
                return NotFound();
            }
            return Ok(updatedAlbum);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var result = await _albumService.DeleteAlbumAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
