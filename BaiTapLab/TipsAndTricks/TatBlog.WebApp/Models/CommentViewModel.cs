using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Models
{
    public class CommentViewModel
    {
        public int PostId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }
    }
}
