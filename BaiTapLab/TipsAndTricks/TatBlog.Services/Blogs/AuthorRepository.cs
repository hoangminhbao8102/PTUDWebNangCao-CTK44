using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public AuthorRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public Task<bool> AddOrUpdateAsync(Author author, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        // a. Tạo interface IAuthorRepository và lớp AuthorRepository.
        // e. Thêm hoặc cập nhật thông tin một tác giả.
        public async Task<bool> AddOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default)
        {
            if (author.Id > 0)
            {
                _context.Authors.Update(author);
            }
            else
            {
                _context.Authors.Add(author);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteAuthorAsync(int id, CancellationToken cancellationToken = default)
        {
            var author = await _context.Authors.FindAsync(new object[] { id }, cancellationToken);
            if (author == null) return false;

            _context.Authors.Remove(author);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        // b. Tìm một tác giả theo mã số.
        public async Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Include(a => a.Posts)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            return await _context.Set<Author>().FindAsync(authorId);
        }

        // c. Tìm một tác giả theo tên định danh(slug).
        public async Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Include(a => a.Posts)
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }

        public async Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Authors.ToListAsync(cancellationToken);
        }

        public async Task<Author> GetCachedAuthorByIdAsync(int authorId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"author.by-id.{authorId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetAuthorByIdAsync(authorId);
                });
        }

        public async Task<Author> GetCachedAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"author.by-slug.{slug}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetAuthorBySlugAsync(slug, cancellationToken);
                });
        }

        // d. Lấy và phân trang danh sách tác giả kèm theo số lượng bài viết của tác giả đó. Kết quả trả về kiểu IPagedList<AuthorItem>.
        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var query = _context.Authors
                .Select(a => new AuthorItem
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    UrlSlug = a.UrlSlug,
                    ImageUrl = a.ImageUrl,
                    JoinedDate = a.JoinedDate,
                    Email = a.Email,
                    Notes = a.Notes,
                    PostCount = a.Posts.Count
                });

            return await query.ToPagedListAsync(pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
        {
            var authorsQuery = _context.Set<Author>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                authorsQuery = authorsQuery.Where(x => x.FullName.Contains(name));
            }

            return await authorsQuery
                .Select(a => new AuthorItem
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    JoinedDate = a.JoinedDate,
                    ImageUrl = a.ImageUrl,
                    UrlSlug = a.UrlSlug,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedAuthorsAsync<T>(Func<IQueryable<Author>, IQueryable<T>> mapper, IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
        {
            var authorQuery = _context.Set<Author>().AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
            }

            return await mapper(authorQuery)
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        // f.Tìm danh sách N tác giả có nhiều bài viết nhất.N là tham số đầu vào.
        public async Task<IList<AuthorItem>> GetTopAuthorsAsync(int count, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .OrderByDescending(a => a.Posts.Count)
                .Take(count)
                .Select(a => new AuthorItem
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    UrlSlug = a.UrlSlug,
                    ImageUrl = a.ImageUrl,
                    JoinedDate = a.JoinedDate,
                    Email = a.Email,
                    Notes = a.Notes,
                    PostCount = a.Posts.Count
                }).ToListAsync(cancellationToken);
        }

        public async Task<bool> IsAuthorSlugExistedAsync(int authorId, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .AnyAsync(x => x.Id != authorId && x.UrlSlug == slug, cancellationToken);
        }

        public async Task<bool> SetImageUrlAsync(int authorId, string imageUrl, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Where(x => x.Id == authorId)
                .ExecuteUpdateAsync(x =>
                x.SetProperty(a => a.ImageUrl, a => imageUrl),
                cancellationToken) > 0;
        }
    }
}
