namespace TatBlog.Core.Entities
{
    public class PostTag
    {
        public int PostId { get; set; }
        public int TagId { get; set; }

        // Navigation Properties
        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
