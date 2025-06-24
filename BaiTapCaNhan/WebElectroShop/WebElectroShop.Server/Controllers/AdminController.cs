using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebElectroShop.Server.Data.Contexts;

namespace WebElectroShop.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly ElectroShopDbContext _context;
        public AdminController(ElectroShopDbContext context) => _context = context;

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var totalOrders = await _context.Orders.CountAsync();
            var totalProducts = await _context.Products.CountAsync();
            var totalUsers = await _context.Users.CountAsync();
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending");

            return Ok(new
            {
                totalOrders,
                totalProducts,
                totalUsers,
                pendingOrders
            });
        }
    }
}
