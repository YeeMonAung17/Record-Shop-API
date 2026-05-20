
using System.ComponentModel.DataAnnotations;

namespace Record_Shop.Models
{
   

    public class Album
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Artist is required.")]
        public required string Artist { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        public required string Genre { get; set; }

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
        public int Year { get; set; }

        [Range(0, 9999, ErrorMessage = "Price must be a non-negative value.")]
        public int Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative value.")]
        public int Stock { get; set; }

        public string? ImageUrl { get; set; }
    }
}
