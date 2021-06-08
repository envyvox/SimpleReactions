using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SR.Data.Enums;
using SR.Data.Models;
using SR.Framework.EF;

namespace SR.Data.Configuration
{
    public class DiscordGuildConfiguration : EntityTypeConfigurationBase<DiscordGuild>
    {
        public override void Configure(EntityTypeBuilder<DiscordGuild> b)
        {
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.Prefix).IsRequired().HasDefaultValue("..");
            b.Property(x => x.Language).IsRequired().HasDefaultValue(Language.English);
            b.Property(x => x.Color).IsRequired().HasDefaultValue("36393F");

            base.Configure(b);
        }
    }
}
