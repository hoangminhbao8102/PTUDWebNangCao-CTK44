using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebElectroShop.Server.Core.Entities;

namespace WebElectroShop.Server.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.Role)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.IsActive)
                   .IsRequired()
                   .HasDefaultValue(false);
        }
    }
}
