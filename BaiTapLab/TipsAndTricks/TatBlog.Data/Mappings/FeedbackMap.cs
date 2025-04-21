using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
    public class FeedbackMap : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedbacks");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(f => f.Subject)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(f => f.Message)
                .IsRequired();

            builder.Property(f => f.SentDate)
                .HasColumnType("datetime");
        }
    }
}
