using Microsoft.EntityFrameworkCore;
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

        public AuthorRepository(BlogDbContext context)
        {
            _context = context;
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
    }
}
