using Microsoft.EntityFrameworkCore;
using Record_Shop.Models;

namespace Record_Shop.Data
{
    public class RecordDbContext : DbContext
    {

        public DbSet<Album> Albums { get; set; }

        public RecordDbContext(DbContextOptions<RecordDbContext> options) : base(options) { }

      

    }


}
