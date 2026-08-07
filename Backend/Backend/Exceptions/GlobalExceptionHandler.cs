using Backend.Exceptions.Repository;
using Backend.Exceptions.User;
using Microsoft.AspNetCore.Diagnostics;

namespace Backend.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var(statusCode, message) = exception switch
            {
                AppException ex => (ex.StatusCode, ex.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(new { error = message }, cancellationToken);
            return true;
        }
    }
}
