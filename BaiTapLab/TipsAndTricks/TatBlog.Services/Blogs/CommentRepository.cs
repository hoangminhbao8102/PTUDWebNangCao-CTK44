using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext _context;

        public CommentRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task AddCommentAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            await _context.Comments.AddAsync(comment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> AddOrUpdateAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            if (comment.Id > 0)
                _context.Update(comment);
            else
                _context.Add(comment);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> ApproveCommentAsync(int id, CancellationToken cancellationToken = default)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (comment == null)
                return false;

            comment.IsApproved = true;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (comment == null)
                return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IList<Comment>> GetAllCommentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Comments
                .OrderByDescending(c => c.PostedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Comment> GetCommentByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IList<Comment>> GetCommentsAsync(int postId, bool approvedOnly = true, CancellationToken cancellationToken = default)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId && (!approvedOnly || c.IsApproved))
                .ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<CommentItem>> GetPagedCommentsAsync(CommentFilterModel model, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Comment>()
                .AsNoTracking()
                .Include(c => c.Post)
                .WhereIf(model.PostId > 0, c => c.PostId == model.PostId)
                .WhereIf(!string.IsNullOrWhiteSpace(model.Keyword),
                         c => c.Content.Contains(model.Keyword))
                .WhereIf(model.ShowOnlyApproved, c => c.IsApproved)
                .OrderByDescending(c => c.PostedDate) // ✅ Sửa lại tên đúng
                .Select(c => new CommentItem()
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    PostTitle = c.Post.Title,
                    Content = c.Content,
                    AuthorName = c.AuthorName,       // ✅ Sửa lại tên đúng
                    Email = c.Email,
                    IsApproved = c.IsApproved,
                    PostedDate = c.PostedDate        // ✅ Sửa lại tên đúng
                })
                .ToPagedListAsync(model, cancellationToken);
        }

        public async Task<IList<Comment>> GetPendingCommentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Comments
                .Where(c => !c.IsApproved)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Comment>> GetUnapprovedCommentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Comments
                .Where(c => !c.IsApproved)
                .OrderByDescending(c => c.PostedDate)
                .ToListAsync(cancellationToken);
        }
    }
}
