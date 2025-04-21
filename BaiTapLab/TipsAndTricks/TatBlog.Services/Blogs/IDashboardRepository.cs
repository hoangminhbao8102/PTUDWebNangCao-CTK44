namespace TatBlog.Services.Blogs
{
    public interface IDashboardRepository
    {
        Task<int> CountTotalPostsAsync(CancellationToken cancellationToken = default);

        Task<int> CountUnpublishedPostsAsync(CancellationToken cancellationToken = default);

        Task<int> CountCategoriesAsync(CancellationToken cancellationToken = default);

        Task<int> CountAuthorsAsync(CancellationToken cancellationToken = default);

        Task<int> CountPendingCommentsAsync(CancellationToken cancellationToken = default);

        Task<int> CountSubscribersAsync(CancellationToken cancellationToken = default);

        Task<int> CountNewSubscribersTodayAsync(CancellationToken cancellationToken = default);
    }
}
