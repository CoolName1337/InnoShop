using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyHttpClient(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddSingleton<ITokenProvider, TokenProvider>();
                
            services.AddTransient<AuthHeaderHandler>();

            services.AddHttpClient<MyHttpClient>(client => {
                client.BaseAddress = new Uri(configuration["ProductService:BaseUrl"]);
            }).AddHttpMessageHandler<AuthHeaderHandler>();
            return services;
        }
        public static void AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                opt =>
                {
                    opt.TokenValidationParameters = new()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };
                    opt.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["nyam-nyam"];

                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireRole("Admin");
                });

                options.AddPolicy("CanEditProduct", policy =>
                {
                    policy.RequireClaim("permission", "CanEditProduct");
                });

                options.AddPolicy("CanDeleteUser", policy =>
                {
                    policy.RequireClaim("permission", "CanDeleteUser");
                });
            });
        }
    }
}
