using WebElectroShop.Server.Core.DTO;
using WebElectroShop.Server.Core.Entities;

namespace WebElectroShop.Server.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<Product>> SearchAsync(string? keyword, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
    }
}
