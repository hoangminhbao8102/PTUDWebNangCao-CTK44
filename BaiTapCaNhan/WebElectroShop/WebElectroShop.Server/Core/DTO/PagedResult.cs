namespace WebElectroShop.Server.Core.DTO
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageNumber { get; set; }          // Trang hiện tại
        public int PageSize { get; set; }            // Số item mỗi trang
        public int TotalItems { get; set; }          // Tổng số item trong toàn bộ truy vấn
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public int FirstItemIndex => (PageNumber - 1) * PageSize + 1;
        public int LastItemIndex => Math.Min(PageNumber * PageSize, TotalItems);
    }
}
