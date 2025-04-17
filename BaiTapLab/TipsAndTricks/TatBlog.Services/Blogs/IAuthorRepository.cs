﻿using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IAuthorRepository
    {
        // a. Tạo interface IAuthorRepository và lớp AuthorRepository.
        // b. Tìm một tác giả theo mã số.
        Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        // c. Tìm một tác giả theo tên định danh(slug).
        Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default);

        // d. Lấy và phân trang danh sách tác giả kèm theo số lượng bài viết của tác giả đó. Kết quả trả về kiểu IPagedList<AuthorItem>.
        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

        // e. Thêm hoặc cập nhật thông tin một tác giả.
        Task<bool> AddOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default);

        // f. Tìm danh sách N tác giả có nhiều bài viết nhất.N là tham số đầu vào.
        Task<IList<AuthorItem>> GetTopAuthorsAsync(int count, CancellationToken cancellationToken = default);

        // Bổ sung thêm
        Task<bool> DeleteAuthorAsync(int id, CancellationToken cancellationToken = default);

        Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default); // cho view đơn giản

        Task<Author> GetCachedAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<Author> GetAuthorByIdAsync(int authorId);

        Task<Author> GetCachedAuthorByIdAsync(int authorId);

        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedAuthorsAsync<T>(Func<IQueryable<Author>, IQueryable<T>> mapper, IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateAsync(Author author, CancellationToken cancellationToken = default);

        Task<bool> IsAuthorSlugExistedAsync(int authorId, string slug, CancellationToken cancellationToken = default);

        Task<bool> SetImageUrlAsync(int authorId, string imageUrl, CancellationToken cancellationToken = default);
    }
}
