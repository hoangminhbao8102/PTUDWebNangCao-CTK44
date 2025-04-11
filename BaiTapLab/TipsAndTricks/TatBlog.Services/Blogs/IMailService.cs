namespace TatBlog.Services.Blogs
{
    public interface IMailService
    {
        Task SendMailAsync(string subject, string body, CancellationToken cancellationToken = default);
    }
}
