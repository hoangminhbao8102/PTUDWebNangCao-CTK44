using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddCategoryAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateCategoryAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }
    }
}
