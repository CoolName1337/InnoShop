using Infrastructure.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using UserService.API.Extensions;
using UserService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Configure(builder.Configuration.GetSection("Kestrel"));
//});
builder.WebHost.UseKestrel()
    .UseUrls("http://+:5001");

builder.Services.AddMyHttpClient(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddEmailService(builder.Configuration);

builder.Services.AddUserServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHadlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always
});

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();
