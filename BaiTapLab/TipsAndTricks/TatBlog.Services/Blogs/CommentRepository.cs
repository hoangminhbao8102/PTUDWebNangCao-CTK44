using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

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
