using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogDbContext _context;

        public CategoryRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await GetCategoryByIdAsync(id, cancellationToken);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Categories.ToListAsync(cancellationToken);
        }

        public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Categories.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
