#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordleS.Models;

namespace WordleS.Data
{
    public class WordleSContext : DbContext
    {
        public WordleSContext (DbContextOptions<WordleSContext> options)
            : base(options)
        {
        }

        public DbSet<WordleS.Models.Users> Users { get; set; }
    }
}
