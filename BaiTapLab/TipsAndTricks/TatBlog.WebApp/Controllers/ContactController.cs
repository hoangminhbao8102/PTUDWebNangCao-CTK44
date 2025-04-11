using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Models;

namespace TatBlog.WebApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IMailService _mailService;

        public ContactController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var subject = $"[Góp ý từ {model.FullName}]";
            var body = $"<p><strong>Email:</strong> {model.Email}</p><p><strong>Nội dung:</strong><br/>{model.Message}</p>";

            await _mailService.SendMailAsync(subject, body);

            ViewBag.Success = "Cảm ơn bạn đã góp ý! Chúng tôi sẽ phản hồi sớm.";
            return View();
        }
    }
}
