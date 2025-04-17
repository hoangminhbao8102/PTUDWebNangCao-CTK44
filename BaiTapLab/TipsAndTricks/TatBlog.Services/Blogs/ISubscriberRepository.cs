using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ISubscriberRepository
    {
        // Đăng ký theo dõi
        Task SubscribeAsync(string email, CancellationToken cancellationToken = default);
        
        // Hủy đăng ký
        Task UnsubscribeAsync(string email, string reason, bool voluntary, CancellationToken cancellationToken = default);
        
        // Chặn một người theo dõi
        Task BlockSubscriberAsync(int id, string reason, string notes, CancellationToken cancellationToken = default);

        // Xóa một người theo dõi
        Task DeleteSubscriberAsync(int id, CancellationToken cancellationToken = default);

        // Tìm người theo dõi bằng ID
        Task<Subscriber> GetSubscriberByIdAsync(int id, CancellationToken cancellationToken = default);

        // Tìm người theo dõi bằng email
        Task<Subscriber> GetSubscriberByEmailAsync(string email, CancellationToken cancellationToken = default);

        // Tìm danh sách người theo dõi theo nhiều tiêu chí khác nhau
        Task<IPagedList<Subscriber>> SearchSubscribersAsync(IPagingParams pagingParams, string keyword, bool? unsubscribed, bool? involuntary, CancellationToken cancellationToken = default);

        Task<IList<Subscriber>> GetSubscribersAsync(CancellationToken cancellationToken = default);

        Task<bool> AddSubscriberAsync(string email, CancellationToken cancellationToken = default);

        Task<bool> RemoveSubscriberAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> ToggleSubscriberStatusAsync(int id, CancellationToken cancellationToken = default);

        Task<int> CountSubscribersAsync(CancellationToken cancellationToken = default);

        Task<int> CountNewSubscribersTodayAsync(CancellationToken cancellationToken = default);
    }
}
