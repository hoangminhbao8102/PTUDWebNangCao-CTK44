
using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly BlogDbContext _context;

        public DashboardRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAuthorsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Authors.CountAsync(cancellationToken);
        }

        public async Task<int> CountCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Categories.CountAsync(cancellationToken);
        }

        public async Task<int> CountNewSubscribersTodayAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.Today;
            return await _context.Subscribers
                .CountAsync(s => s.SubscribedDate >= today, cancellationToken);
        }

        public async Task<int> CountPendingCommentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Comments.CountAsync(c => !c.IsApproved, cancellationToken);
        }

        public async Task<int> CountSubscribersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Subscribers.CountAsync(cancellationToken);
        }

        public async Task<int> CountTotalPostsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Posts.CountAsync(cancellationToken);
        }

        public async Task<int> CountUnpublishedPostsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Posts.CountAsync(p => !p.Published, cancellationToken);
        }
    }
}
