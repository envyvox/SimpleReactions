using Microsoft.EntityFrameworkCore;
using SR.Data.Entities;
using SR.Data.Extensions;

namespace SR.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.UseEntityTypeConfiguration<AppDbContext>();
            modelBuilder.UseSnakeCaseNamingConvention();
        }

        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<DiscordGuild> DiscordGuilds { get; set; }
    }
}
