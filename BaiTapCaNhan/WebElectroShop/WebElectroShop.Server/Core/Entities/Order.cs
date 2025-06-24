using WebElectroShop.Server.Core.Contracts;

namespace WebElectroShop.Server.Core.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public ICollection<OrderItem>? Items { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string Status { get; set; } = "Chưa giải quyết"; // "Đã xác nhận", "Đang vận chuyển", "Đã giao hàng", "Đã hủy"
        public bool IsPaid { get; set; } = false;
    }
}
