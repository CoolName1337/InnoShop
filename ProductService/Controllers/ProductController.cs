using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Extensions;
using ProductService.Contracts.DTOs;
using ProductService.Contracts.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService, ILogger<ProductController> logger) : ControllerBase
    {
        /// <summary>Возвращает все продукты.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken ct)
        {
            var res = await productService.GetAllAsync(ct);
            return Ok(res);
        }

        /// <summary>Возвращает продукт по ID.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken ct)
        {
            var res = await productService.GetByIdAsync(id, ct);
            return Ok(res);
        }   
        [HttpGet("/{ownerId}")]
        public async Task<IActionResult> GetByOwnerId([FromRoute] int ownerId, CancellationToken ct)
        {
            var res = await productService.GetByOwnerIdAsync(ownerId, ct);

            return Ok(res);
        }

        /// <summary>Создаёт новый продукт.</summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] CreateProductDTO createProductDTO, CancellationToken ct)
        {
            if (User.TryGetUserId(out int userId))
            {
                var res = await productService.CreateAsync(createProductDTO, userId, ct);
                return Created();
            }
            else
            {
                return Unauthorized("Missing or invalid userId claim");
            }
        }

        /// <summary>Обновляет продукт.</summary>
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> PatchAsync([FromBody] UpdateProductDTO updateProductDTO, CancellationToken ct)
        {
            if (User.TryGetUserId(out int userId))
            {
                var res = await productService.UpdateAsync(updateProductDTO, userId, ct);
                return Ok(res);
            }
            else
            {
                return Unauthorized("Missing or invalid userId claim");
            }
        }

        /// <summary>Удаляет продукт по ID.</summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            if (User.TryGetUserId(out int userId))
            {
                await productService.DeleteAsync(id, userId, ct);
                return NoContent();
            }
            else
            {
                return Unauthorized("Missing or invalid userId claim");
            }
        }

        [HttpPost("admin/soft-delete/{ownerId}&{isDeleted}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> SetDeleted(int ownerId, bool isDeleted, CancellationToken ct)
        {
            var connectedProductIds = await productService.SetDeletedByOwnerIdAsync(ownerId, isDeleted, ct);

            return Ok(connectedProductIds);
        }
    }
}
