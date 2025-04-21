using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly BlogDbContext _context;

        public FeedbackRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFeedbackAsync(Feedback feedback, CancellationToken cancellationToken = default)
        {
            _context.Feedbacks.Add(feedback);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
