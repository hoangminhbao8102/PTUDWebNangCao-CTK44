using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public class Subscriber : IEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public DateTime SubscribedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UnsubscribedDate { get; set; }

        public string UnsubscribeReason { get; set; }

        public bool? Involuntary { get; set; }

        public string AdminNotes { get; set; }
    }
}
