namespace TatBlog.Core.DTO
{
    public class CommentItem
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public DateTime PostedDate { get; set; }

        public bool IsApproved { get; set; }

        public int PostId { get; set; }

        public string PostTitle { get; set; } // lấy từ Post.Title
    }
}
