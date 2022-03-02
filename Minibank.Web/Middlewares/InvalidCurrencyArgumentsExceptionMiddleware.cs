using Microsoft.AspNetCore.Http;
using Minibank.Core.Services;
using System.Threading.Tasks;

namespace Minibank.Web.Middlewares
{
    public class InvalidCurrencyArgumentsExceptionMiddleware
    {
        public readonly RequestDelegate _next;

        public InvalidCurrencyArgumentsExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (InvalidCurrencyArgumentsException ex)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Message = ex.Message });
            }
        }
    }
}
