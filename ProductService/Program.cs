using Microsoft.EntityFrameworkCore;
using ProductService.API.Extensions;
using ProductService.API.Middlewares;
using ProductService.BLL;
using ProductService.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProductServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Add exception handler to middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
