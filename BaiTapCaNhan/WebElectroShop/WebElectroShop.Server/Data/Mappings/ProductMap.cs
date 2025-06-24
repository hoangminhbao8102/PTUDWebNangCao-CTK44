using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebElectroShop.Server.Core.Entities;

namespace WebElectroShop.Server.Data.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .HasColumnType("nvarchar(max)");

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.ImageUrl)
                .HasMaxLength(500);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.Stock)
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products) // nếu trong Category có public ICollection<Product> Products { get; set; }
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
