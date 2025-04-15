using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }

        // r. Đếm số lượng bài viết thỏa điều kiện PostQuery
        public async Task<int> CountPostsByQueryAsync(PostQuery query, CancellationToken cancellationToken = default)
        {
            var posts = FilterPosts(query);
            return await posts.CountAsync(cancellationToken);
        }

        // k. Đếm số lượng bài viết trong N tháng gần nhất
        public async Task<IList<MonthlyPostCount>> CountPostsInRecentMonthsAsync(int numberOfMonths, CancellationToken cancellationToken = default)
        {
            var fromDate = DateTime.Now.AddMonths(-numberOfMonths);

            return await _context.Posts
                .Where(p => p.Published && p.PostedDate >= fromDate)
                .GroupBy(p => new { p.PostedDate.Year, p.PostedDate.Month })
                .Select(g => new MonthlyPostCount
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    PostCount = g.Count()
                })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .ToListAsync(cancellationToken);
        }

        // g. Thêm hoặc cập nhật một chuyên mục/chủ đề
        public async Task<Category> CreateOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
        {
            if (category.Id > 0)
            {
                _context.Categories.Update(category);
            }
            else
            {
                await _context.Categories.AddAsync(category, cancellationToken);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return category;
        }

        // m. Thêm hay cập nhật một bài viết
        public async Task<Post> CreateOrUpdatePostAsync(Post post, CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
                _context.Posts.Update(post);
            else
                await _context.Posts.AddAsync(post, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return post;
        }

        public async Task<Post> CreateOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default)
        {
            // Chuẩn hóa danh sách tag (loại bỏ trùng lặp, khoảng trắng)
            var tagList = tags
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Lấy danh sách các tag đã tồn tại trong CSDL
            var existingTags = await _context.Tags
                .Where(t => tagList.Contains(t.Name))
                .ToListAsync(cancellationToken);

            // Tìm hoặc tạo mới các tag còn thiếu
            var newTags = tagList
                .Where(name => existingTags.All(t => !t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                .Select(name => new Tag
                {
                    Name = name,
                    UrlSlug = GenerateSlug(name),
                    Description = name
                })
                .ToList();

            // Gộp danh sách tất cả các tag
            var allTags = existingTags.Concat(newTags).ToList();

            // Gán danh sách tag cho bài viết
            post.Tags = allTags;

            // Thêm mới hoặc cập nhật bài viết
            if (post.Id > 0)
                _context.Posts.Update(post);
            else
                await _context.Posts.AddAsync(post, cancellationToken);

            // Thêm các tag mới vào DbContext
            if (newTags.Any())
                await _context.Tags.AddRangeAsync(newTags, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return post;
        }

        // h. Xóa một chuyên mục theo mã số cho trước
        public async Task<bool> DeleteCategoryByIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _context.Categories.FindAsync(new object[] { categoryId }, cancellationToken);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        // d. Xóa một thẻ theo mã cho trước
        public async Task<bool> DeleteTagByIdAsync(int tagId, CancellationToken cancellationToken = default)
        {
            var tag = await _context.Tags.FindAsync(new object[] { tagId }, cancellationToken);
            if (tag == null) return false;

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        // c.Lấy danh sách tất cả các thẻ kèm theo số bài viết chứa thẻ đó, trả về IList<TagItem>
        public async Task<IList<TagItem>> GetAllTagsWithPostCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tags
                .Select(t => new TagItem
                {
                    Id = t.Id,
                    Name = t.Name,
                    UrlSlug = t.UrlSlug,
                    Description = t.Description,
                    PostCount = t.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Include(a => a.Posts) // nếu bạn muốn kèm danh sách bài viết
                .ToListAsync(cancellationToken);
        }

        // Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục/chủ đề
        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        // f. Tìm một chuyên mục theo mã số cho trước
        public async Task<Category> GetCategoryByIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _context.Categories.FindAsync(new object[] { categoryId }, cancellationToken);
        }

        // e. Tìm một chuyên mục (Category) theo tên định danh (slug)
        public async Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.UrlSlug == slug, cancellationToken);
        }

        // j. Lấy và phân trang danh sách chuyên mục, kết quả trả về kiểu IPagedList<CategoryItem>
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var query = _context.Categories
                .Select(c => new CategoryItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlSlug = c.UrlSlug,
                    Description = c.Description,
                    ShowOnMenu = c.ShowOnMenu,
                    PostCount = c.Posts.Count(p => p.Published)
                });

            return await query.ToPagedListAsync(pagingParams, cancellationToken);
        }

        // s. Phân trang các bài viết theo PostQuery
        public async Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var posts = FilterPosts(query);
            return await posts.ToPagedListAsync(pagingParams, cancellationToken);
        }

        // t. Phân trang với ánh xạ sang kiểu T (generic)
        public async Task<IPagedList<T>> GetPagedPostsAsync<T>(PostQuery query, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            var posts = FilterPosts(query);
            var projected = mapper(posts);
            return await projected.ToPagedListAsync(pagingParams, cancellationToken);
        }

        // Nâng cấp bài tập 1s
        public async Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize, 
                nameof(Post.PostedDate), "DESC", 
                cancellationToken);
        }

        // Lấy danh sách từ khòa/thẻ và phân trang theo các tham số pagingParams
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        // Tìm Top N bài viết phổ biến được nhiều người xem nhất
        public async Task<List<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        // Tìm bài viết có tên định danh là 'slug' và được đăng và tháng 'month' năm 'year'
        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);

            if (year > 0) 
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }

            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }

            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }

            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        // l. Tìm một bài viết theo mã số
        public async Task<Post> GetPostByIdAsync(int postId, CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
        }

        public async Task<Post> GetPostByIdAsync(int postId, bool includeDetails, CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
        }

        // q. Tìm tất cả bài viết thỏa điều kiện trong PostQuery
        public async Task<IList<Post>> GetPostsByQueryAsync(PostQuery query, CancellationToken cancellationToken = default)
        {
            var posts = FilterPosts(query);
            return await posts.ToListAsync(cancellationToken);
        }

        // o. Lấy ngẫu nhiên N bài viết
        public async Task<IList<Post>> GetRandomPostsAsync(int numberOfPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .Where(p => p.Published)
                .OrderBy(p => Guid.NewGuid())
                .Take(numberOfPosts)
                .ToListAsync(cancellationToken);
        }

        // a. Tìm một thẻ (Tag) theo tên định danh (slug)
        public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.UrlSlug == slug, cancellationToken);
        }

        // Tăng số lượt xem của một bài viết
        public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        // i. Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa
        public async Task<bool> IsCategorySlugExistedAsync(int categoryId, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AnyAsync(c => c.Id != categoryId && c.UrlSlug == slug, cancellationToken);
        }

        // Kiểm tra xem tên định danh của bài viết đã có hay chưa
        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }

        // n. Chuyển đổi trạng thái Published của bài viết
        public async Task<bool> TogglePublishedStatusAsync(int postId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Posts.FindAsync(new object[] { postId }, cancellationToken);
            if (post == null) return false;

            post.Published = !post.Published;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        // 🧩 Hỗ trợ method FilterPosts
        private IQueryable<Post> FilterPosts(PostQuery query)
        {
            IQueryable<Post> posts = _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Include(p => p.Tags);

            if (query.AuthorId.HasValue)
                posts = posts.Where(p => p.AuthorId == query.AuthorId);

            if (query.CategoryId.HasValue)
                posts = posts.Where(p => p.CategoryId == query.CategoryId);

            if (!string.IsNullOrWhiteSpace(query.CategorySlug))
                posts = posts.Where(p => p.Category.UrlSlug == query.CategorySlug);

            if (query.Year.HasValue)
                posts = posts.Where(p => p.PostedDate.Year == query.Year);

            if (query.Month.HasValue)
                posts = posts.Where(p => p.PostedDate.Month == query.Month);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
                posts = posts.Where(p =>
                    p.Title.Contains(query.Keyword) ||
                    p.ShortDescription.Contains(query.Keyword) ||
                    p.Description.Contains(query.Keyword));

            return posts;
        }

        private static string GenerateSlug(string name)
        {
            return name.ToLowerInvariant().Replace(" ", "-").Replace(".", "").Replace(",", "");
        }

        // b. Tạo lớp DTO TagItem để chứa thông tin về thẻ và số lượng bài viết chứa thẻ đó (Đã tạo lớp TagItem ở TatBlog.Core.DTO)

        // p. Tạo lớp PostQuery để lưu trữ điều kiện tìm kiếm (Đã tạo lớp PostQuery ở TatBlog.Core.DTO)
    }
}
