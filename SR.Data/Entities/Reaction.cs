using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SR.Data.Util;

namespace SR.Data.Entities
{
    public class Reaction : IUniqueIdentifiedEntity
    {
        public Guid Id { get; set; }
        public Enums.ReactionType Type { get; set; }
        public string Url { get; set; }
    }

    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.Type, x.Url }).IsUnique();

            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Url).IsRequired();
        }
    }
}
