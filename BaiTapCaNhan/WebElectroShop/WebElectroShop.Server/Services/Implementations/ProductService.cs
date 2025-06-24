using Microsoft.EntityFrameworkCore;
using WebElectroShop.Server.Core.DTO;
using WebElectroShop.Server.Core.Entities;
using WebElectroShop.Server.Core.Extensions;
using WebElectroShop.Server.Data.Contexts;
using WebElectroShop.Server.Services.Interfaces;

namespace WebElectroShop.Server.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ElectroShopDbContext _context;

        public ProductService(ElectroShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _context.Products.Include(p => p.Category).ToListAsync(cancellationToken);

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Products.FindAsync(product.Id);
            if (existing == null) return false;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.ImageUrl = product.ImageUrl;
            existing.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<PagedResult<Product>> SearchAsync(string? keyword, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(p => p.Name!.Contains(keyword));

            return await query.ToPagedResultAsync(page, pageSize, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync(cancellationToken);
        }
    }
}
