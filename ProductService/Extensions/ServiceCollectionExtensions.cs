using Microsoft.EntityFrameworkCore;
using ProductService.BLL;
using ProductService.Contracts.Interfaces;
using ProductService.DAL;
using ProductService.DAL.Interfaces;
using ProductService.DAL.Repositories;

namespace ProductService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Тут построим Пятерочку.
        /// </summary>
        /// <param name="configuration">builder.Configuration чувак</param>
        public static IServiceCollection AddProductServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Регистрируем DbContext
            services.AddDbContext<ProductDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("ProductsDb"));
            });
            
            // Регистрируем репозитории
            services.AddScoped<IProductRepository, ProductRepository>();
            
            // Регистрируем сервисы
            services.AddScoped<IProductService, BLL.Services.ProductService>(); //((((((((
            
            // Регистрируем маппер
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ProductProfile).Assembly);
            });

            return services;
        }
    }
}
