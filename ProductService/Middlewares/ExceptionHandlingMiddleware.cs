using FluentValidation;
using ProductService.BLL.Exceptions;
using System.Text.Json;

namespace ProductService.API.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning("NOT FOUND: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogWarning("UNAUTHORIZED ACCESS: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var result = JsonSerializer.Serialize(new
                {
                    error = "Validation error",
                    errors = ex.Errors.Select(e => e.ErrorMessage)
                });
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UNHANDLED EXCEPTION");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "Internal Server Error" });
            }
        }
    }
}
