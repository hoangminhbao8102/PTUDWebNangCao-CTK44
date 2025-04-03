namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        public int? AuthorId { get; set; }

        public int? CategoryId { get; set; }

        public string CategorySlug { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public string Keyword { get; set; }
    }
}
