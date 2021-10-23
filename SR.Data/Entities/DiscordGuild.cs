using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SR.Data.Enums;

namespace SR.Data.Entities
{
    public class DiscordGuild
    {
        public long Id { get; set; }
        public LanguageType LanguageType { get; set; }
        public string EmbedColor { get; set; }
    }

    public class DiscordGuildConfiguration : IEntityTypeConfiguration<DiscordGuild>
    {
        public void Configure(EntityTypeBuilder<DiscordGuild> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.LanguageType).IsRequired();
            builder.Property(x => x.EmbedColor).IsRequired();
        }
    }
}
