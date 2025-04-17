using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _commentRepository.GetAllCommentsAsync();
            return View(comments);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var success = await _commentRepository.ApproveCommentAsync(id);
            TempData["Message"] = success ? "Phê duyệt thành công." : "Không tìm thấy bình luận.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var success = await _commentRepository.DeleteCommentAsync(id);
            TempData["Message"] = success ? "Xóa bình luận thành công." : "Không tìm thấy bình luận.";
            return RedirectToAction(nameof(Index));
        }
    }
}
