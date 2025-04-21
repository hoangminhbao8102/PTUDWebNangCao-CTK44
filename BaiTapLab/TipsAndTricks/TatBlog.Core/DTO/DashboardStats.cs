namespace TatBlog.Core.DTO
{
    public class DashboardStats
    {
        public int TotalPosts { get; set; }

        public int UnpublishedPosts { get; set; }

        public int TotalCategories { get; set; }

        public int TotalAuthors { get; set; }

        public int PendingComments { get; set; }

        public int TotalSubscribers { get; set; }

        public int NewSubscribersToday { get; set; }
    }
}
