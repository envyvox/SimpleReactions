using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SR.Data.Models;
using SR.Framework.EF;

namespace SR.Data.Configuration
{
    public class ReactionConfiguration : EntityTypeConfigurationBase<Reaction>
    {
        public override void Configure(EntityTypeBuilder<Reaction> b)
        {
            b.HasIndex(x => new {x.Type, x.Url}).IsUnique();

            base.Configure(b);
        }
    }
}
