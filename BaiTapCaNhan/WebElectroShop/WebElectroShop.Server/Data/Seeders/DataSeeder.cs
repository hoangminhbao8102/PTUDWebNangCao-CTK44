using WebElectroShop.Server.Core.Entities;
using WebElectroShop.Server.Data.Contexts;

namespace WebElectroShop.Server.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly ElectroShopDbContext _dbContext;

        public DataSeeder(ElectroShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (!_dbContext.Users.Any())
            {
                AddUsers();
            }

            if (_dbContext.Categories.Any() || _dbContext.Products.Any())
                return;

            var categories = AddCategories();
            var products = AddProducts(categories);
        }

        private IList<Category> AddCategories()
        {
            var categories = new List<Category>
            {
                new() { Name = "Smartphones", UrlSlug = "smartphones", Description = "Various smartphones from brands" },
                new() { Name = "Laptops", UrlSlug = "laptops", Description = "High performance laptops" },
                new() { Name = "Accessories", UrlSlug = "accessories", Description = "Phone & laptop accessories" }
            };

            _dbContext.Categories.AddRange(categories);
            _dbContext.SaveChanges();

            return categories;
        }

        private IList<Product> AddProducts(IList<Category> categories)
        {
            var products = new List<Product>
            {
                new() { Name = "iPhone 15", Price = 999.99m, CategoryId = categories[0].Id, Description = "Latest iPhone", ImageUrl = "/images/iphone15.jpg" },
                new() { Name = "Samsung Galaxy S24", Price = 899.99m, CategoryId = categories[0].Id, Description = "Newest Samsung flagship", ImageUrl = "/images/galaxys24.jpg" },
                new() { Name = "Dell XPS 15", Price = 1499.99m, CategoryId = categories[1].Id, Description = "Powerful Windows laptop", ImageUrl = "/images/dellxps15.jpg" },
                new() { Name = "MacBook Pro 14", Price = 1999.99m, CategoryId = categories[1].Id, Description = "Apple's high-end laptop", ImageUrl = "/images/macbookpro14.jpg" },
                new() { Name = "Wireless Charger", Price = 29.99m, CategoryId = categories[2].Id, Description = "Qi certified wireless charger", ImageUrl = "/images/charger.jpg" }
            };

            _dbContext.Products.AddRange(products);
            _dbContext.SaveChanges();

            return products;
        }

        private void AddUsers()
        {
            var users = new List<User>
            {
                new()
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin"
                },
                new()
                {
                    Username = "user",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                    Role = "User"
                }
            };

            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();
        }
    }
}
