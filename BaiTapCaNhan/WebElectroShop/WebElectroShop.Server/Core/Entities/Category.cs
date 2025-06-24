using WebElectroShop.Server.Core.Contracts;

namespace WebElectroShop.Server.Core.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? UrlSlug { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
