using Microsoft.EntityFrameworkCore;

namespace GatauWebSite.Model
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options):base(options)
        {

        }

        public DbSet <Product> Products { get; set; }

    }
}
   
