using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.BLL;
using UserService.BLL.Services;
using UserService.Contracts.Interfaces;
using UserService.DAL;
using UserService.DAL.Interfaces;
using UserService.DAL.Repositories;

namespace UserService.API.Extensions
{
    public static class UserServicesExtensions
    {
        public static void AddUserServices(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
#if !TEST
            services.AddDbContext<UserDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("UsersDb"));
            });
#endif
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IUserService, BLL.Services.UserService>(); // =(

            services.AddScoped<IAdminService, AdminService>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(UserProfile).Assembly);
            });

        }
        public static void AddEmailService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
