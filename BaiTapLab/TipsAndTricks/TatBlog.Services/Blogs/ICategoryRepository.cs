using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ICategoryRepository
    {
        Task<IList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
        Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default);
        Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
        Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);
    }
}
