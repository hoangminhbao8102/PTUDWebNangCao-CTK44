using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;

        public PostsController(ILogger<PostsController> logger, IBlogRepository blogRepository, IAuthorRepository authorRepository, IMediaManager mediaManager, IMapper mapper)
        {
            _logger = logger;
            _blogRepository = blogRepository;
            _authorRepository = authorRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
        }

        public IActionResult AjaxIndex()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index(PostFilterModel model, int pageNumber = 1)
        {
            _logger.LogInformation("Tạo điều kiện truy vấn bài viết...");

            // Ánh xạ từ model sang PostQuery
            var postQuery = _mapper.Map<PostQuery>(model);
            postQuery.PublishedOnly = !model.Unpublished;

            // Lấy dữ liệu bài viết đã phân trang
            var posts = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, 10);

            // Chuẩn bị dữ liệu cho dropdown (tác giả, chủ đề...)
            await PopulatePostFilterModelAsync(model);

            var result = new PostFilterResultModel
            {
                Filter = model,
                Posts = posts
            };

            return View(result);
        }

        private async Task PopulatePostFilterModelAsync(PostFilterModel model)
        {
            var authors = await _authorRepository.GetAuthorsAsync();
            var categories = await _blogRepository.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });

            model.CategoryList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            // ID = 0 <=> Thêm bài viết mới
            // ID > 0 : Đọc dữ liệu của bài viết từ CSDL
            var post = id > 0 
                ? await _blogRepository.GetPostByIdAsync(id, true) 
                : null;

            // Tạo view model từ dữ liệu của bài viết
            var model = post == null ? new PostEditModel() : _mapper.Map<PostEditModel>(post);

            // Gán các giá trị khác cho view model
            await PopulatePostEditModelAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IValidator<PostEditModel> postValidator, PostEditModel model)
        {
            var validationResult = await postValidator.ValidateAsync(model);

            if (validationResult.IsValid) 
            {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid)
            {
                await PopulatePostEditModelAsync(model);
                return View(model);
            }

            var post = model.Id > 0 
                ? await _blogRepository.GetPostByIdAsync(model.Id) 
                : null;

            if (post == null)
            {
                post = _mapper.Map<Post>(model);

                post.Id = 0;
                post.PostedDate = DateTime.Now;
            }
            else
            {
                _mapper.Map(model, post);

                post.Category = null;
                post.ModifiedDate = DateTime.Now;
            }

            // Nếu người dùng có upload hình ảnh minh họa cho bài viết
            if (model.ImageFile?.Length > 0) 
            {
                // Thì thực hiện việc lưu tập tinh vào thư mục uploads
                var newImagePath = await _mediaManager.SaveFileAsync(
                    model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);

                // Nếu lưu thành công, xóa tập tin hình ảnh cũ (nếu có)
                if (!string.IsNullOrWhiteSpace(newImagePath))
                {
                    await _mediaManager.DeleteFileAsync(post.ImageUrl);
                    post.ImageUrl = newImagePath;
                }
            }

            await _blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulatePostEditModelAsync(PostEditModel model)
        {
            var authors = await _blogRepository.GetAuthorsAsync();
            var categories = await _blogRepository.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });

            model.CategoryList = categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPostSlug(int id, string urlSlug)
        {
            var slugExisted = await _blogRepository.IsPostSlugExistedAsync(id, urlSlug);

            return slugExisted 
                ? Json($"Slug '{urlSlug}' đã được sử dụng") 
                : Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> TogglePublish(int id)
        {
            var success = await _blogRepository.TogglePublishedStatusAsync(id);

            if (!success)
            {
                TempData["ErrorMessage"] = "Không thể thay đổi trạng thái xuất bản cho bài viết.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _blogRepository.DeletePostByIdAsync(id);

            if (!success)
            {
                TempData["ErrorMessage"] = "Không tìm thấy bài viết để xóa.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> GetPostData()
        {
            var requestForm = Request.Form;

            int skip = int.Parse(requestForm["start"]);
            int pageSize = int.Parse(requestForm["length"]);
            string searchValue = requestForm["search[value]"];
            string sortColumn = requestForm["columns[" + requestForm["order[0][column]"] + "][data]"];
            string sortDirection = requestForm["order[0][dir]"];

            var postQuery = new PostQuery
            {
                Keyword = searchValue
            };

            var posts = await _blogRepository.GetPagedPostsAsync(
                postQuery,
                pageNumber: skip / pageSize + 1,
                pageSize: pageSize
            );

            return Json(new
            {
                draw = requestForm["draw"],
                recordsTotal = posts.TotalItemCount,
                recordsFiltered = posts.TotalItemCount,
                data = posts.Select(p => new
                {
                    title = p.Title,
                    categoryName = p.Category.Name,
                    authorName = p.Author.FullName,
                    postedDate = p.PostedDate.ToString("dd/MM/yyyy"),
                    published = p.Published
                })
            });
        }
    }
}
