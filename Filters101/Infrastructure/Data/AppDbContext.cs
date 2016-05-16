using Filters101.Models;
using Microsoft.EntityFrameworkCore;

namespace Filters101.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options )
            :base(options)
        {
        }
        public DbSet<Author> Authors { get; set; }
    }
}
