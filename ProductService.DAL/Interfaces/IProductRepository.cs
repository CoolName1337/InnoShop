using ProductService.Contracts.Filters;
using ProductService.DAL.Domain;
using ProductService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.DAL.Interfaces
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllAsync(CancellationToken cancellationToken, bool includeDeleted = false);
        public Task<List<Product>> GetAllByFilterAsync(ProductFilter productFilter, CancellationToken cancellationToken);
        public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<Product> CreateAsync(Product product, CancellationToken cancellationToken);
        public Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);
        public Task DeleteAsync(int id, CancellationToken cancellationToken);
        public Task SoftDeleteAsync(int id, CancellationToken cancellationToken);
        public Task<(List<Product>, int)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken, bool includeDeleted = false);
    }
}
