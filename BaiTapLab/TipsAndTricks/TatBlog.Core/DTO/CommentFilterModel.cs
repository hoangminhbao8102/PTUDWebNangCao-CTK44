using TatBlog.Core.Contracts;

namespace TatBlog.Core.DTO
{
    public class CommentFilterModel : PagingModel, IPagingParams
    {
        public int PostId { get; set; }

        public string Keyword { get; set; }

        public bool ShowOnlyApproved { get; set; } = false;
    }
}
