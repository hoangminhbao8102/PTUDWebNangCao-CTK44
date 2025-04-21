using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public class Feedback : IEntity
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Subject { get; set; } = "";

        public string Message { get; set; } = "";

        public DateTime SentDate { get; set; } = DateTime.UtcNow;
    }
}
