using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagsController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public TagsController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _blogRepository.GetTagsAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            var tag = id > 0 ? await _blogRepository.GetTagByIdAsync(id) : new Tag();
            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Tag tag)
        {
            if (!ModelState.IsValid)
                return View(tag);

            await _blogRepository.AddOrUpdateTagAsync(tag);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogRepository.DeleteTagAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
