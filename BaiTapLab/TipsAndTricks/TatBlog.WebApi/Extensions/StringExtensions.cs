namespace TatBlog.WebApi.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateSlug(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.ToLowerInvariant();

            // Loại bỏ ký tự không phải chữ và số
            var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(input);
            input = System.Text.Encoding.ASCII.GetString(bytes);

            input = System.Text.RegularExpressions.Regex.Replace(input, @"\s", "-");       // khoảng trắng => dấu gạch ngang
            input = System.Text.RegularExpressions.Regex.Replace(input, @"[^a-z0-9\s-]", ""); // ký tự đặc biệt
            input = System.Text.RegularExpressions.Regex.Replace(input, @"-+", "-");      // gộp dấu gạch ngang

            return input.Trim('-');
        }
    }
}
