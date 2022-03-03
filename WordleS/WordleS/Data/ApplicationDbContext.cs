using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WordleS.Models;

namespace WordleS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Word> Word { get; set; }

        public DbSet<Game> Game { get; set; }

        public DbSet<Attempt> Attempt { get; set; }
    }
}