using Microsoft.AspNetCore.Mvc;

namespace WebElectroShop.Server.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file");

            var path = Path.Combine("wwwroot/images", file.FileName);
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            var url = $"/images/{file.FileName}";
            return Ok(new { url });
        }
    }
}
