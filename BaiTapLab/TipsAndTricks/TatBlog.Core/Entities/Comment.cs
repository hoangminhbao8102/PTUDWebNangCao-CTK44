using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;

        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
