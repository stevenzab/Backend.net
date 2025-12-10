using Microsoft.EntityFrameworkCore;
using PlayerBack.Models;

namespace PlayerBack.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PlayerModel> PlayerModels { get; set; }
    }
}
