using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BlogDbContext _context;

        public SubscriberRepository(BlogDbContext context) 
        {
            _context = context;
        }

        // Chặn một người theo dõi
        public async Task BlockSubscriberAsync(int id, string reason, string notes, CancellationToken cancellationToken = default)
        {
            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (subscriber != null)
            {
                subscriber.UnsubscribedDate = DateTime.UtcNow;
                subscriber.UnsubscribeReason = reason;
                subscriber.Involuntary = true;
                subscriber.AdminNotes = notes;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        // Xóa một người theo dõi
        public async Task DeleteSubscriberAsync(int id, CancellationToken cancellationToken = default)
        {
            var subscriber = await _context.Subscribers.FindAsync(new object[] { id }, cancellationToken);

            if (subscriber != null)
            {
                _context.Subscribers.Remove(subscriber);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        // Tìm người theo dõi bằng email
        public async Task<Subscriber> GetSubscriberByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Subscribers.FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
        }

        // Tìm người theo dõi bằng ID
        public async Task<Subscriber> GetSubscriberByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Subscribers.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        // Tìm danh sách người theo dõi theo nhiều tiêu chí khác nhau
        public async Task<IPagedList<Subscriber>> SearchSubscribersAsync(IPagingParams pagingParams, string keyword, bool? unsubscribed, bool? involuntary, CancellationToken cancellationToken = default)
        {
            var query = _context.Subscribers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(s => s.Email.Contains(keyword));

            if (unsubscribed.HasValue)
                query = query.Where(s => (s.UnsubscribedDate != null) == unsubscribed.Value);

            if (involuntary.HasValue)
                query = query.Where(s => s.Involuntary == involuntary.Value);

            return await query
                .OrderByDescending(s => s.SubscribedDate)
                .ToPagedListAsync(pagingParams.PageNumber, pagingParams.PageSize);
        }

        // Đăng ký theo dõi
        public async Task SubscribeAsync(string email, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.Email == email, cancellationToken);

            if (existing == null)
            {
                var sub = new Subscriber { Email = email };
                _context.Subscribers.Add(sub);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else if (existing.UnsubscribedDate != null)
            {
                existing.UnsubscribedDate = null;
                existing.UnsubscribeReason = null;
                existing.Involuntary = null;
                existing.AdminNotes = null;
                existing.SubscribedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        // Hủy đăng ký
        public async Task UnsubscribeAsync(string email, string reason, bool voluntary, CancellationToken cancellationToken = default)
        {
            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.Email == email, cancellationToken);

            if (subscriber != null)
            {
                subscriber.UnsubscribedDate = DateTime.UtcNow;
                subscriber.UnsubscribeReason = reason;
                subscriber.Involuntary = !voluntary;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
