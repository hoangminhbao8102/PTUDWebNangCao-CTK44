using TatBlog.Core.Contracts;

namespace TatBlog.Core.DTO
{
    public class TagFilterModel : PagingModel
    {
        public string Name { get; set; }
    }
}
