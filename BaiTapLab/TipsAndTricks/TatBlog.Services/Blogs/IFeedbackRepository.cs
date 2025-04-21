using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IFeedbackRepository
    {
        Task<bool> AddFeedbackAsync(Feedback feedback, CancellationToken cancellationToken = default);
    }
}
