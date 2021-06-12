using Microsoft.EntityFrameworkCore;
using SR.Data.Models;

namespace SR.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<DiscordGuild> DiscordGuilds { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
