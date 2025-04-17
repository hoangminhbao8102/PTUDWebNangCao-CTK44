using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ISubscriberRepository _subscriberRepository;

        public DashboardController(IBlogRepository blogRepository, ISubscriberRepository subscriberRepository)
        {
            _blogRepository = blogRepository;
            _subscriberRepository = subscriberRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                TotalPosts = await _blogRepository.CountPostsAsync(),
                UnpublishedPosts = await _blogRepository.CountUnpublishedPostsAsync(),
                TotalCategories = await _blogRepository.CountCategoriesAsync(),
                TotalAuthors = await _blogRepository.CountAuthorsAsync(),
                PendingComments = await _blogRepository.CountUnapprovedCommentsAsync(),
                TotalSubscribers = await _subscriberRepository.CountSubscribersAsync(),
                NewSubscribersToday = await _subscriberRepository.CountNewSubscribersTodayAsync()
            };

            return View(model);
        }
    }
}
