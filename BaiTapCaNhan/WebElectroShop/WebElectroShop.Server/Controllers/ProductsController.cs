using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebElectroShop.Server.Core.DTO;
using WebElectroShop.Server.Core.Entities;
using WebElectroShop.Server.Services.Interfaces;

namespace WebElectroShop.Server.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(await _productService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetByCategory(int id)
        {
            return Ok(await _productService.GetByCategoryAsync(id));
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<Product>>> SearchProducts(
            [FromQuery] string? keyword,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _productService.SearchAsync(keyword, page, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            return Ok(await _productService.CreateAsync(product));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();
            return await _productService.UpdateAsync(product) ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) =>
            await _productService.DeleteAsync(id) ? Ok() : NotFound();
    }
}
