using System.Text.Json;
using UserService.BLL.Exceptions;

namespace UserService.API.Middlewares
{
    public class ExceptionHadlerMiddleware(
        RequestDelegate _next,
        ILogger<ExceptionHadlerMiddleware> _logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundUserException ex)
            {
                _logger.LogError(ex.Message);

                context.Response.StatusCode = StatusCodes.Status404NotFound;

                var result = JsonSerializer.Serialize(new
                {
                    error = "User not found",
                    details = ex.Message
                });
                await context.Response.WriteAsync(result);
            }

            catch (FailedToLoginException ex)
            {
                _logger.LogError(ex.Message);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                var result = JsonSerializer.Serialize(new
                {
                    error = "Failed to login",
                    details = ex.Message
                });
                await context.Response.WriteAsync(result);
            }
            catch (FailedToRegisterException ex)
            {
                _logger.LogError(ex.Message);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var result = JsonSerializer.Serialize(new
                {
                    error = "Failed to register",
                    details = ex.Message
                });
                await context.Response.WriteAsync(result);
            }
            catch (FailedToSendEmail ex)
            {
                _logger.LogError(ex.Message);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var result = JsonSerializer.Serialize(new
                {
                    error = "Failed to send email",
                    details = ex.Message
                });
                await context.Response.WriteAsync(result);

            }
            catch (FailedToValidate ex)
            {
                _logger.LogError(ex.Message);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var result = JsonSerializer.Serialize(new
                {
                    error = "Failed to validate",
                    details = ex.Message
                });
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var result = JsonSerializer.Serialize(new
                {
                    error = "Something went wrong",
                    details = ex.Message
                });

                await context.Response.WriteAsync(result);
            }
        }

    }
}
