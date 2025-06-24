using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebElectroShop.Server.Core.DTO;
using WebElectroShop.Server.Core.Entities;
using WebElectroShop.Server.Data.Contexts;
using WebElectroShop.Server.Services.Interfaces;

namespace WebElectroShop.Server.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly ElectroShopDbContext _context;
        private readonly IOrderService _orderService;

        public OrdersController(ElectroShopDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            var result = await _orderService.CreateOrderAsync(order);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            return await _orderService.ConfirmOrderAsync(id) ? Ok() : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/paid")]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.IsPaid = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent(
            [FromServices] IHttpClientFactory httpClientFactory,
            [FromBody] PaymentRequest model)
        {
            var client = httpClientFactory.CreateClient("Stripe");

            var parameters = new Dictionary<string, string>
            {
                { "amount", ((int)(model.Amount * 100)).ToString() },  // convert to cents
                { "currency", "usd" },
                { "payment_method_types[]", "card" }
            };

            var content = new FormUrlEncodedContent(parameters);

            var response = await client.PostAsync("payment_intents", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonString);
            var clientSecret = doc.RootElement.GetProperty("client_secret").GetString();

            return Ok(new { clientSecret });
        }
    }
}
