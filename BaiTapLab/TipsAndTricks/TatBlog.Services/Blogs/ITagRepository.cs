using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ITagRepository
    {
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            TagFilterModel filter,
            string name,
            CancellationToken cancellationToken = default);

        Task<Tag> GetCachedTagByIdAsync(int tagId, CancellationToken cancellationToken = default);

        Task<Tag> GetTagByIdAsync(int tagId, CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateAsync(Tag tag, CancellationToken cancellationToken = default);

        Task<bool> DeleteTagAsync(int tagId, CancellationToken cancellationToken = default);

        Task<bool> IsTagSlugExistedAsync(int tagId, string slug, CancellationToken cancellationToken = default);
    }
}
