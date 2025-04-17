using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly ISubscriberRepository _subscriberRepository;

        public SubscribersController(ISubscriberRepository subscriberRepository)
        {
            _subscriberRepository = subscriberRepository;
        }

        public async Task<IActionResult> Index()
        {
            var subscribers = await _subscriberRepository.GetSubscribersAsync();
            return View(subscribers);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                await _subscriberRepository.AddSubscriberAsync(email);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await _subscriberRepository.RemoveSubscriberAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _subscriberRepository.ToggleSubscriberStatusAsync(id);
            return RedirectToAction("Index");
        }
    }
}
