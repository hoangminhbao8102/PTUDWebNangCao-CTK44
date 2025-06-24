using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebElectroShop.Server.Core.Entities;
using WebElectroShop.Server.Data.Contexts;

namespace WebElectroShop.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/category")]
    public class CategoriesController : ControllerBase
    {
        private readonly ElectroShopDbContext _context;
        public CategoriesController(ElectroShopDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() =>
    Ok(await _context.Categories.ToListAsync());

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, Category updated)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null) return NotFound();

            cat.Name = updated.Name;
            cat.Description = updated.Description;
            await _context.SaveChangesAsync();
            return Ok(cat);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null) return NotFound();

            _context.Categories.Remove(cat);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
