namespace TatBlog.WebApi.Models
{
    public class SubscriberEditModel
    {
        public string Email { get; set; }

        public string Reason { get; set; } = "Người dùng tự hủy theo dõi";

        public bool Voluntary { get; set; } = true;
    }
}
