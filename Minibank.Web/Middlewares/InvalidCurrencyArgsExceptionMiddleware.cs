using Microsoft.AspNetCore.Http;
using Minibank.Core.Services;
using System.Threading.Tasks;

namespace Minibank.Web.Middlewares
{
    public class InvalidCurrencyArgsExceptionMiddleware
    {
        public readonly RequestDelegate _next;

        public InvalidCurrencyArgsExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (InvalidCurrencyArgsException ex)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Message = ex.Message });
            }
        }
    }
}
