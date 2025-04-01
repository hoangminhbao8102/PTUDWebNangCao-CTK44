using TatBlog.Core.Contracts;

// Biểu diễn tác giả của một bài viết
namespace TatBlog.Core.Entities
{
    public class Author : IEntity
    {
        // Mã tác giả bài viết
        public int Id { get; set; }

        // Tên tác giả
        public string FullName { get; set; }

        // Tên định danh dùng để tạo URL
        public string UrlSlug { get; set; }

        // Đường dẫn tới file hình ảnh
        public string ImageUrl { get; set; }

        // Ngày bắt đầu
        public DateTime JoinedDate { get; set; }

        // Địa chỉ Email
        public string Email { get; set; }

        // Ghi chú
        public string Notes { get; set; }

        // Danh sách các bài viết của tác giả
        public IList<Post> Posts { get; set; }
    }
}
