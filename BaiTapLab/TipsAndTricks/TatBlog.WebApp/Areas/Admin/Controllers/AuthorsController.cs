using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string sortColumn = "FullName",
            string sortOrder = "ASC",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var pagingParams = new PagingParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortOrder = sortOrder
            };

            var authors = await _authorRepository.GetPagedAuthorsAsync(pagingParams, name: null);

            return View(authors);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {
                await _authorRepository.AddOrUpdateAuthorAsync(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(id);
            if (author == null) return NotFound();
            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                await _authorRepository.AddOrUpdateAuthorAsync(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(id);
            if (author == null) return NotFound();
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _authorRepository.DeleteAuthorAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
