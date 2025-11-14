using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Domain;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces;

namespace ProductService.DAL.Repositories
{
    public class ProductRepository(ProductDbContext db) : IProductRepository
    {
        public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
        {
            await db.Products.AddAsync(product, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var product = await db.Products.FindAsync(id, cancellationToken);
            if (product != null)
            {
                db.Products.Remove(product);
                await db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken, bool includeDeleted = false)
        {
            var query = db.Products.AsQueryable();
            if (!includeDeleted)
                query = query.Where(pr => !pr.IsDeleted);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetAllByFilterAsync(ProductFilter productFilter, CancellationToken cancellationToken)
        {
            var query = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(productFilter.ContainString))
                query = query.Where(pr =>
                pr.Name.Contains(productFilter.ContainString) || pr.Description.Contains(productFilter.ContainString));

            if (productFilter.MinPrice is decimal minPrice)
                query = query.Where(pr => pr.Price >= minPrice);
            if (productFilter.MaxPrice is decimal maxPrice)
                query = query.Where(pr => pr.Price <= maxPrice);

            if (productFilter.FromDate is DateOnly fromDate)
                query = query.Where(pr => pr.CreatedDate >= fromDate);
            if (productFilter.ToDate is DateOnly toDate)
                query = query.Where(pr => pr.CreatedDate <= toDate);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await db.Products.FindAsync(id, cancellationToken);
        }

        public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            db.Products.Update(product);
            await db.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task SoftDeleteAsync(int id, CancellationToken cancellationToken)
        {
            var product = await db.Products.FindAsync(id, cancellationToken);
            if (product != null)
            {
                product.IsDeleted = true;
                db.Products.Update(product);
                await db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<(List<Product>, int)> GetPagedAsync(
            int pageNumber, int pageSize, CancellationToken cancellationToken, bool includeDeleted = false)
        {
            var query = db.Products.AsQueryable();

            if(!includeDeleted)
                query = query.Where((p) => !p.IsDeleted);

            var totalCount = await query.CountAsync(cancellationToken);

            var products = await query
                .OrderBy(pr => pr.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (products, totalCount);
        }
    }
}
