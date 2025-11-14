using Microsoft.AspNetCore.Mvc;
using ProductService.Contracts.DTOs;
using ProductService.Contracts.Interfaces;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        /// <summary>Возвращает все продукты.</summary>
        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetAllAsync(CancellationToken ct)
        {
            var res = await productService.GetAllAsync(ct);
            return Ok(res);
        }

        /// <summary>Возвращает продукт по ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetAsync(int id, CancellationToken ct)
        {
            var res = await productService.GetByIdAsync(id, ct);
            return Ok(res);
        }

        /// <summary>Создаёт новый продукт.</summary>
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostAsync([FromBody] CreateProductDTO createProductDTO, CancellationToken ct)
        {
            var res = await productService.CreateAsync(createProductDTO, ct);
            return CreatedAtAction(nameof(GetAsync), new { id = res.Id }, res);
        }

        /// <summary>Обновляет продукт.</summary>
        [HttpPatch]
        public async Task<ActionResult<ProductDTO>> PatchAsync([FromBody] UpdateProductDTO updateProductDTO, CancellationToken ct)
        {
            var res = await productService.UpdateAsync(updateProductDTO, ct);
            return Ok(res);
        }

        /// <summary>Удаляет продукт по ID.</summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken ct)
        {
            await productService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
