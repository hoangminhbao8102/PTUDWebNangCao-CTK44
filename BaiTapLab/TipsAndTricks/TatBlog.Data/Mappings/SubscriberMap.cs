using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
    public class SubscriberMap : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.ToTable("Subscribers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.SubscribedDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(x => x.UnsubscribedDate)
                .HasColumnType("datetime");

            builder.Property(x => x.UnsubscribeReason)
                .HasMaxLength(1000);

            builder.Property(x => x.Involuntary)
                .HasDefaultValue(false);

            builder.Property(x => x.AdminNotes)
                .HasMaxLength(1000);
        }
    }
}
