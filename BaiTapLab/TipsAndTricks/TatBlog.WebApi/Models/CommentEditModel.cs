namespace TatBlog.WebApi.Models
{
    public class CommentEditModel
    {
        public int PostId { get; set; }

        public string AuthorName { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public bool IsApproved { get; set; } = false;
    }
}
