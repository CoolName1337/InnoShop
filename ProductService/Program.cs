using Infrastructure.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using ProductService.API.Extensions;
using ProductService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Configure(builder.Configuration.GetSection("Kestrel"));
//});
builder.WebHost.UseKestrel()
    .UseUrls("http://+:5000");

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddProductServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Add exception handler to middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always
});

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();
