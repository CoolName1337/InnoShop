using ProductService.Contracts.DTOs;

namespace ProductService.Contracts.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDTO?> FindByIdAsync(int id, CancellationToken ct);
        public Task<ProductDTO> GetByIdAsync(int id, CancellationToken ct); 
        public Task<List<ProductDTO>> GetAllAsync(CancellationToken ct); 
        public Task<ProductDTO> CreateAsync(CreateProductDTO createProductDTO, CancellationToken ct);
        public Task<ProductDTO> UpdateAsync(UpdateProductDTO updateProductDTO, CancellationToken ct);
        public Task DeleteAsync(int id, CancellationToken ct);
        public Task<PagedResult<ProductDTO>> GetPagedResult(int page, int pageSize, CancellationToken ct);
    }
}
