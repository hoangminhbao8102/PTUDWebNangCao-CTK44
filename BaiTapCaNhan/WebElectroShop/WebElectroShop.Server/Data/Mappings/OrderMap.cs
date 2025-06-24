using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebElectroShop.Server.Core.Entities;

namespace WebElectroShop.Server.Data.Mappings
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Đặt tên bảng
            builder.ToTable("Orders");

            // Khóa chính
            builder.HasKey(x => x.Id);

            // Ánh xạ OrderDate, đặt mặc định là GETDATE()
            builder.Property(x => x.OrderDate)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("GETDATE()");

            // Ánh xạ CustomerName
            builder.Property(x => x.CustomerName)
                   .HasMaxLength(100);

            // Ánh xạ CustomerEmail
            builder.Property(x => x.CustomerEmail)
                   .HasMaxLength(100);

            // Ánh xạ Status
            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("Chưa giải quyết");

            // Ánh xạ IsPaid với mặc định false
            builder.Property(x => x.IsPaid)
                   .HasDefaultValue(false);

            // Thiết lập quan hệ với OrderItem (1-n)
            builder.HasMany(x => x.Items)
                   .WithOne()
                   .HasForeignKey("OrderId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
