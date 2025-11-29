using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductService.DAL
{
    public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

            optionsBuilder.UseSqlServer(
                "Data Source=localhost\\SQLEXPRESS01;Database=ProductsDb;Integrated Security=True;Persist Security Info=False;" +
                "Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=30"
                );

            return new ProductDbContext(optionsBuilder.Options);
        }
    }
}
