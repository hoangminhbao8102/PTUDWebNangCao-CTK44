using WebElectroShop.Server.Core.Entities;

namespace WebElectroShop.Server.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task<bool> ConfirmOrderAsync(int orderId, CancellationToken cancellationToken = default);
        Task SendOrderConfirmationAsync(Order order, CancellationToken cancellationToken = default);
    }
}
