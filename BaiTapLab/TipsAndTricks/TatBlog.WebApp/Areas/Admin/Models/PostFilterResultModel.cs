using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class PostFilterResultModel
    {
        public PostFilterModel Filter { get; set; }
        public IPagedList<Post> Posts { get; set; }
    }
}
