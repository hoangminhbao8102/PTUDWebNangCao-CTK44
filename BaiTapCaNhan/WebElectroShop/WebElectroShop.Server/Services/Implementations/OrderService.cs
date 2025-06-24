using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using WebElectroShop.Server.Core.Entities;
using WebElectroShop.Server.Data.Contexts;
using WebElectroShop.Server.Services.Interfaces;

namespace WebElectroShop.Server.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ElectroShopDbContext _context;
        private readonly IConfiguration _config;

        public OrderService(ElectroShopDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);
            return order;
        }

        public async Task<bool> ConfirmOrderAsync(int orderId, CancellationToken cancellationToken = default)
        {
            // Lấy đơn hàng bao gồm danh sách sản phẩm trong đơn
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (order == null)
                return false;

            // Kiểm tra và cập nhật tồn kho sản phẩm
            foreach (var item in order.Items!)
            {
                var product = await _context.Products.FindAsync(new object[] { item.ProductId }, cancellationToken);
                if (product == null || product.Stock < item.Quantity)
                    throw new Exception($"Sản phẩm ID {item.ProductId} không đủ hàng trong kho.");

                product.Stock -= item.Quantity;
            }

            // Cập nhật trạng thái đơn hàng
            order.Status = "Đã xác nhận";
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task SendOrderConfirmationAsync(Order order, CancellationToken cancellationToken = default)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Shop", _config["Smtp:User"]));
            email.To.Add(MailboxAddress.Parse(order.CustomerEmail ?? "customer@gmail.com")); // fallback nếu null
            email.Subject = $"Xác nhận đơn hàng #{order.Id}";
            email.Body = new TextPart("plain")
            {
                Text = $"Cảm ơn bạn đã đặt hàng. Tổng cộng: {order.Items!.Sum(x => x.Quantity * x.Product!.Price):N0} VND"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]!), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["Smtp:User"], _config["Smtp:Pass"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
