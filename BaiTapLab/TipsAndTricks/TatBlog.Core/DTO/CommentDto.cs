namespace TatBlog.Core.DTO
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public DateTime PostedDate { get; set; }
    }
}
