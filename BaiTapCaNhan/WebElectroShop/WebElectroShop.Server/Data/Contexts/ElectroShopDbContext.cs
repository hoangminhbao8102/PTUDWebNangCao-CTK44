using Microsoft.EntityFrameworkCore;
using WebElectroShop.Server.Core.Entities;
using WebElectroShop.Server.Data.Mappings;

namespace WebElectroShop.Server.Data.Contexts
{
    public class ElectroShopDbContext : DbContext
    {
        public ElectroShopDbContext(DbContextOptions<ElectroShopDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ánh xạ các entity với Fluent API
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new OrderItemMap());

            // Nếu có ánh xạ Category
            if (typeof(CategoryMap).IsAssignableTo(typeof(IEntityTypeConfiguration<Category>)))
            {
                modelBuilder.ApplyConfiguration(new CategoryMap());
            }

            // ✅ Ánh xạ User
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
