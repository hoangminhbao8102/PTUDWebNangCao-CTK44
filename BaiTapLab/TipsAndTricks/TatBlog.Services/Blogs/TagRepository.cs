using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public TagRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<bool> AddOrUpdateAsync(Tag tag, CancellationToken cancellationToken = default)
        {
            if (tag.Id > 0)
            {
                _context.Tags.Update(tag);
            }
            else
            {
                _context.Tags.Add(tag);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteTagAsync(int tagId, CancellationToken cancellationToken = default)
        {
            return await _context.Tags
                .Where(t => t.Id == tagId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<Tag> GetCachedTagByIdAsync(int tagId, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"tag.by-id.{tagId}",
                async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetTagByIdAsync(tagId, cancellationToken);
                });
        }

        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(TagFilterModel filter, string name, CancellationToken cancellationToken = default)
        {
            if (filter == null)
            {
                filter = new TagFilterModel();
            }

            return await _context.Tags
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(name),
                         t => t.Name.Contains(name))
                .Select(t => new TagItem()
                {
                    Id = t.Id,
                    Name = t.Name,
                    UrlSlug = t.UrlSlug,
                    Description = t.Description,
                    PostCount = t.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync((IPagingParams)filter, cancellationToken);
        }

        public async Task<Tag> GetTagByIdAsync(int tagId, CancellationToken cancellationToken = default)
        {
            return await _context.Tags.FindAsync(new object[] { tagId }, cancellationToken);
        }

        public async Task<bool> IsTagSlugExistedAsync(int tagId, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Tags
                .AnyAsync(t => t.Id != tagId && t.UrlSlug == slug, cancellationToken);
        }
    }
}
