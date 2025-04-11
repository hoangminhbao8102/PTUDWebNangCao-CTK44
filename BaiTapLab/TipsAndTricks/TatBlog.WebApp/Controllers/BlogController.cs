using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Models;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMailService _mailService;

        public BlogController(IBlogRepository blogRepository, ICommentRepository commentRepository, IMailService mailService)
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _mailService = mailService;
        }

        // Action này xử lý HTTP request dến trang chủ của ứng dụng web hoặc tìm kiếm bài viết theo từ khóa
        public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");

            // Tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // Chỉ lấy những bài viết có trạng thái Published
                PublishedOnly = true,

                // Tìm bài viết theo từ khóa
                Keyword = keyword
            };

            // Truy vấn các bài viết theo diều kiệm đã tạo
            var postsList = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            // Lưu lại điều kiện truy vấn để hiển thị trong View
            ViewBag.PostQuery = postQuery;

            // Truyền danh sách bài viết vào View để render ra HTML
            return View(postsList);
        }

        public async Task<IActionResult> Category(string slug)
        {
            var category = await _blogRepository.GetCategoryBySlugAsync(slug);

            if (category == null)
            {
                return NotFound();
            }

            var query = new PostQuery
            {
                CategorySlug = slug,
                PublishedOnly = true
            };

            var posts = await _blogRepository.GetPostsByQueryAsync(query);
            ViewBag.Category = category;

            return View(posts);
        }

        public async Task<IActionResult> Author(string slug)
        {
            var query = new PostQuery
            {
                AuthorSlug = slug,
                PublishedOnly = true
            };

            var posts = await _blogRepository.GetPostsByQueryAsync(query);

            if (posts == null || !posts.Any())
            {
                return NotFound();
            }

            ViewBag.AuthorSlug = slug;
            ViewBag.AuthorName = posts.FirstOrDefault()?.Author?.FullName ?? "Không rõ";
            return View(posts);
        }

        public async Task<IActionResult> Tag(string slug)
        {
            var tag = await _blogRepository.GetTagBySlugAsync(slug);

            if (tag == null)
            {
                return NotFound();
            }

            var query = new PostQuery
            {
                TagSlug = slug,
                PublishedOnly = true
            };

            var posts = await _blogRepository.GetPostsByQueryAsync(query);
            ViewBag.Tag = tag;

            return View(posts);
        }

        public async Task<IActionResult> Post(int year, int month, int day, string slug)
        {
            var post = await _blogRepository.GetPostAsync(year, month, slug);

            if (post == null || !post.Published)
            {
                return NotFound("Bài viết không tồn tại hoặc chưa được xuất bản.");
            }

            await _blogRepository.IncreaseViewCountAsync(post.Id);

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin.";
                return RedirectToAction("Post", new { id = model.PostId });
            }

            var comment = new Comment
            {
                AuthorName = model.Name,
                Content = model.Content,
                PostId = model.PostId,
                PostedDate = DateTime.Now,
                IsApproved = false
            };

            await _commentRepository.AddCommentAsync(comment);

            TempData["Success"] = "Bình luận của bạn đã được gửi và chờ kiểm duyệt.";
            return RedirectToAction("Post", new { id = model.PostId });
        }

        public async Task<IActionResult> Archives(int year, int month)
        {
            var query = new PostQuery
            {
                Year = year,
                Month = month,
                PublishedOnly = true
            };

            var posts = await _blogRepository.GetPostsByQueryAsync(query);
            ViewBag.Year = year;
            ViewBag.Month = month;

            return View(posts);
        }

        public IActionResult About()
            => View();

        public IActionResult Contact()
            => View();

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var subject = $"[Góp ý từ {model.FullName}]";
            var body = $"<p><strong>Email:</strong> {model.Email}</p><p><strong>Nội dung:</strong><br/>{model.Message}</p>";

            await _mailService.SendMailAsync(subject, body);

            ViewBag.Success = "Cảm ơn bạn đã góp ý! Chúng tôi sẽ phản hồi sớm.";
            return View();
        }

        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhật");
    }
}
