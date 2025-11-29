using ProductService.DAL.Domain;
using ProductService.DAL.Entities;

namespace ProductService.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken, bool includeDeleted = false);
        Task<List<Product>> GetByFilterAsync(ProductFilter productFilter, CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Product> CreateAsync(Product product, CancellationToken cancellationToken);
        Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task SoftDeleteAsync(int id, CancellationToken cancellationToken);
        Task<(List<Product>, int)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken, bool includeDeleted = false);
        Task<List<Product>> GetByOwnerIdAsync(int id, CancellationToken ct);
    }
}
