using WebElectroShop.Server.Core.Contracts;

namespace WebElectroShop.Server.Core.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Stock { get; set; } = 0;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
