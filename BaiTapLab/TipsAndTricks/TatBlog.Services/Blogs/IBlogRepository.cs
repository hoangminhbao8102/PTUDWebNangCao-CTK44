using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        // Tìm bài viết có tên định danh là 'slug' và được đăng và tháng 'month' năm 'year'
        Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default);

        // Tìm Top N bài viết phổ biến được nhiều người xem nhất
        Task<List<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default);

        // Kiểm tra xem tên định danh của bài viết đã có hay chưa
        Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default);

        // Tăng số lượt xem của một bài viết
        Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default);

        // Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục/chủ đề
        Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default);

        // Lấy danh sách từ khòa/thẻ và phân trang theo các tham số pagingParams
        Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

        // a. Tìm một thẻ (Tag) theo tên định danh (slug)
        Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

        // b. Tạo lớp DTO TagItem để chứa thông tin về thẻ và số lượng bài viết chứa thẻ đó (Đã tạo lớp TagItem ở TatBlog.Core.DTO)

        // c. Lấy danh sách tất cả các thẻ kèm theo số bài viết chứa thẻ đó, trả về IList<TagItem>
        Task<IList<TagItem>> GetAllTagsWithPostCountAsync(CancellationToken cancellationToken = default);

        // d. Xóa một thẻ theo mã cho trước
        Task<bool> DeleteTagByIdAsync(int tagId, CancellationToken cancellationToken = default);

        // e. Tìm một chuyên mục (Category) theo tên định danh (slug)
        Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

        // f. Tìm một chuyên mục theo mã số cho trước
        Task<Category> GetCategoryByIdAsync(int categoryId, CancellationToken cancellationToken = default);

        // g. Thêm hoặc cập nhật một chuyên mục/chủ đề
        Task<Category> CreateOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);

        // h. Xóa một chuyên mục theo mã số cho trước
        Task<bool> DeleteCategoryByIdAsync(int categoryId, CancellationToken cancellationToken = default);

        // i. Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa
        Task<bool> IsCategorySlugExistedAsync(int categoryId, string slug, CancellationToken cancellationToken = default);

        // j. Lấy và phân trang danh sách chuyên mục, kết quả trả về kiểu IPagedList<CategoryItem>
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

        // k. Đếm số lượng bài viết trong N tháng gần nhất
        Task<IList<MonthlyPostCount>> CountPostsInRecentMonthsAsync(int numberOfMonths, CancellationToken cancellationToken = default);

        // l. Tìm một bài viết theo mã số
        Task<Post> GetPostByIdAsync(int postId, CancellationToken cancellationToken = default);

        // m. Thêm hay cập nhật một bài viết
        Task<Post> CreateOrUpdatePostAsync(Post post, CancellationToken cancellationToken = default);

        // n. Chuyển đổi trạng thái Published của bài viết
        Task<bool> TogglePublishedStatusAsync(int postId, CancellationToken cancellationToken = default);

        // o. Lấy ngẫu nhiên N bài viết
        Task<IList<Post>> GetRandomPostsAsync(int numberOfPosts, CancellationToken cancellationToken = default);

        // p. Tạo lớp PostQuery để lưu trữ điều kiện tìm kiếm (Đã tạo lớp PostQuery ở TatBlog.Core.DTO)

        // q. Tìm tất cả bài viết thỏa điều kiện trong PostQuery
        Task<IList<Post>> GetPostsByQueryAsync(PostQuery query, CancellationToken cancellationToken = default);

        // r. Đếm số lượng bài viết thỏa điều kiện PostQuery
        Task<int> CountPostsByQueryAsync(PostQuery query, CancellationToken cancellationToken = default);

        // s. Phân trang các bài viết theo PostQuery
        Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        // t. Phân trang với ánh xạ sang kiểu T (generic)
        Task<IPagedList<T>> GetPagedPostsAsync<T>(PostQuery query, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

        // Nâng cấp bài tập 1s
        Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

        Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default);

        Task<Post> GetPostByIdAsync(int postId, bool includeDetails, CancellationToken cancellationToken = default);

        Task<Post> CreateOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);

        // u. Xóa bài viết theo mã số
        Task<bool> DeletePostByIdAsync(int postId, CancellationToken cancellationToken = default);

        Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default);

        Task<Tag> GetTagByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateTagAsync(Tag tag, CancellationToken cancellationToken = default);

        Task<bool> DeleteTagAsync(int id, CancellationToken cancellationToken = default);

        Task<int> CountPostsAsync(CancellationToken cancellationToken = default);

        Task<int> CountUnpublishedPostsAsync(CancellationToken cancellationToken = default);

        Task<int> CountCategoriesAsync(CancellationToken cancellationToken = default);

        Task<int> CountAuthorsAsync(CancellationToken cancellationToken = default);

        Task<int> CountUnapprovedCommentsAsync(CancellationToken cancellationToken = default);

        Task<IPagedList<PostItem>> GetPostsByCategorySlugAsync(string slug, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<Post> GetPostBySlugAsync(string slug, CancellationToken cancellationToken = default);
    }
}
