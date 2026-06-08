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

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetAlbumById(int id)
        {
            var album = await _albumService.GetAlbumByIdAsync(id);
            if (album == null)
            {
                return NotFound(new
                {
                    error = "Album not found",
                    message = $"No album exists with id {id}"
                });
            }
            return Ok(album);
        }

        [HttpGet("artist/{artist}")]
        public async Task<IActionResult> GetAlbumsByArtist(string artist)
        {
            if (string.IsNullOrWhiteSpace(artist))
            {
                return BadRequest("Artist name is required.");
            }
            var albums = await _albumService.GetAlbumsByArtistAsync(artist);
            
            return Ok(albums.ToList());
        }

        [HttpGet("year/{year}")]
        public async Task<IActionResult> GetAlbumsByYear(int year)
        {
            var albums = await _albumService.GetAlbumsByYearAsync(year);
            if (year <= 0)
            {
                return BadRequest("Year must be a positive integer.");
            }
            return Ok(albums.ToList());
        }

        [HttpGet("genre/{genre}")]
        public async Task<IActionResult> GetAlbumsByGenre(string genre)
        {
            var albums = await _albumService.GetAlbumsByGenreAsync(genre);
                        if (string.IsNullOrWhiteSpace(genre))
            {
                return BadRequest("Genre is required."); 
            }
            return Ok(albums.ToList());
        }

        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetAlbumByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Album title is required."); 
            }

            var album = await _albumService.GetAlbumByTitleAsync(title);

            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);
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

            if (album.Id != id)
            {
                return BadRequest(new
                {
                    error = "Id mismatch",
                    message = "The album id in the URL does not match the album id in the request body."
                });
            }

                var updatedAlbum = await _albumService.UpdateAlbumAsync(id, album);

            if (updatedAlbum == null)
            {
                return NotFound(new
                {
                    error = "Album not found",
                    message = $"Cannot update album with id {id} because it does not exist."
                });
            }
            return Ok(updatedAlbum);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var deleted = await _albumService.DeleteAlbumAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    error = "Album not found",
                    message = $"Cannot delete album with id {id} because it does not exist."
                });
            }

            return NoContent();
        }
    }
}
