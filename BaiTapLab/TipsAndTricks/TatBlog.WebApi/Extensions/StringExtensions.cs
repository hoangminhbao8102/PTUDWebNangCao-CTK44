using System.Text;
using System.Text.RegularExpressions;

namespace TatBlog.WebApi.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateSlug(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Không cần mã hóa nếu không làm việc với mã nguồn Cyrillic
            var normalized = input.Normalize(NormalizationForm.FormD);
            var bytes = Encoding.ASCII.GetBytes(normalized); // hoặc Encoding.UTF8
            var ascii = Encoding.ASCII.GetString(bytes);

            // Loại bỏ ký tự không hợp lệ và thay thế khoảng trắng bằng gạch nối
            var slug = Regex.Replace(ascii, @"[^a-zA-Z0-9\s-]", string.Empty);
            slug = Regex.Replace(slug, @"\s+", "-").Trim('-');

            return slug.ToLowerInvariant();
        }
    }
}
