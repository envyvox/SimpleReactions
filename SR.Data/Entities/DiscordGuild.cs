using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SR.Data.Enums;

namespace SR.Data.Entities
{
    public class DiscordGuild
    {
        public long Id { get; set; }
        public string Prefix { get; set; }
        public Language Language { get; set; }
        public string Color { get; set; }
    }

    public class DiscordGuildConfiguration : IEntityTypeConfiguration<DiscordGuild>
    {
        public void Configure(EntityTypeBuilder<DiscordGuild> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Prefix).IsRequired();
            builder.Property(x => x.Language).IsRequired();
            builder.Property(x => x.Color).IsRequired();
        }
    }
}
