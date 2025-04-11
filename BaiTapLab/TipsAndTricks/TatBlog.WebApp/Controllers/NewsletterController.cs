using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IConfiguration _configuration;

        public NewsletterController(ISubscriberRepository subscriberRepository, IConfiguration configuration)
        {
            _subscriberRepository = subscriberRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required");

            var existing = await _subscriberRepository.GetSubscriberByEmailAsync(email);
            if (existing != null)
                return Ok("You are already subscribed");

            await _subscriberRepository.SubscribeAsync(email); // Gọi đúng method

            var unsubscribeUrl = Url.Action("Unsubscribe", "Newsletter", new { email }, Request.Scheme);
            var message = $"Cảm ơn bạn đã đăng ký nhận thông báo.\n\nBạn có thể hủy đăng ký bất cứ lúc nào bằng cách nhấn vào liên kết sau:\n{unsubscribeUrl}";

            await SendEmailAsync(email, "Đăng ký nhận bài viết mới", message);

            return Ok("Đăng ký thành công");
        }

        public async Task<IActionResult> Unsubscribe(string email)
        {
            var subscriber = await _subscriberRepository.GetSubscriberByEmailAsync(email);
            if (subscriber == null)
                return NotFound("Email không tồn tại trong hệ thống.");

            await _subscriberRepository.UnsubscribeAsync(email, "Người dùng tự hủy", voluntary: true);

            return Content("Bạn đã hủy đăng ký nhận thông báo.");
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var from = _configuration["Smtp:From"];
            var host = _configuration["Smtp:Host"];
            var port = int.Parse(_configuration["Smtp:Port"]);
            var user = _configuration["Smtp:Username"];
            var pass = _configuration["Smtp:Password"];

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true // Gmail yêu cầu SSL
            };

            using var message = new MailMessage(from, to, subject, body);
            await client.SendMailAsync(message);
        }
    }
}
