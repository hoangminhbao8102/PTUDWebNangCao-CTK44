namespace TatBlog.Core.DTO
{
    public class PostItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string UrlSlug { get; set; }

        public string ShortDescription { get; set; }

        public string ImageUrl { get; set; }

        public int ViewCount { get; set; }

        public bool Published { get; set; }

        public DateTime PostedDate { get; set; }

        // Tên chuyên mục (nếu cần)
        public string CategoryName { get; set; }

        // Tên tác giả (nếu cần)
        public string AuthorName { get; set; }

        // Số lượng thẻ (nếu cần)
        public int TagCount { get; set; }
    }
}
