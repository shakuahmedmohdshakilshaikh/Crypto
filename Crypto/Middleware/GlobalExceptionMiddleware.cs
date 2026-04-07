using DDDCryptoWebApi.Application.DTO;
using System.Text.Json;
using SendGrid.Helpers.Errors.Model;

namespace Crypto.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Global Exception Occurred");

                context.Response.ContentType = "application/json";

                int statusCode;
                string message;

                switch (ex)
                {
                    case BadRequestException:
                        statusCode = StatusCodes.Status400BadRequest;
                        message = ex.Message;
                        break;

                    case UnauthorizedException:
                        statusCode = StatusCodes.Status401Unauthorized;
                        message = ex.Message;
                        break;

                    case NotFoundException:
                        statusCode = StatusCodes.Status404NotFound;
                        message = ex.Message;
                        break;

                    default:
                        statusCode = StatusCodes.Status500InternalServerError;
                        message = ex.Message;
                        break;
                }

                context.Response.StatusCode = statusCode;

                var response = ApiResponse<string>.ErrorResponse(message);

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response)
                );
            }
        }
    }
}
