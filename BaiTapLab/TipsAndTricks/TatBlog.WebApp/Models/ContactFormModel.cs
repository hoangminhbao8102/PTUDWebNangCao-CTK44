using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên của bạn.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung.")]
        public string Message { get; set; }
    }
}
