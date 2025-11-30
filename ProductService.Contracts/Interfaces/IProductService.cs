using ProductService.Contracts.DTOs;

namespace ProductService.Contracts.Interfaces
{
    public interface IProductService
    {
        Task<ProductDTO?> FindByIdAsync(int id, CancellationToken ct);
        Task<ProductDTO> GetByIdAsync(int id, CancellationToken ct); 
        Task<List<ProductDTO>> GetAllAsync(CancellationToken ct); 
        Task<ProductDTO> CreateAsync(CreateProductDTO createProductDTO, int userId, CancellationToken ct);
        Task<ProductDTO> UpdateAsync(UpdateProductDTO updateProductDTO, int userId, CancellationToken ct);
        Task DeleteAsync(int id, int userId, CancellationToken ct);
        Task<PagedResult<ProductDTO>> GetPagedResult(int page, int pageSize, CancellationToken ct);
        Task<List<ProductDTO>> GetByOwnerIdAsync(int id, CancellationToken ct);
        Task<List<int>> SetDeletedByOwnerIdAsync(int ownerId, bool isDeleted, CancellationToken ct);
        Task<List<ProductDTO>> GetByFilterAsync(FilterProductDTO filterProduct, CancellationToken ct);
    }
}
