using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductService.DAL
{
    public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {
#if !TEST
        public ProductDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ProductsDb");

            optionsBuilder.UseSqlServer(connectionString);

            return new ProductDbContext(optionsBuilder.Options);
        }
    }
#endif
}
