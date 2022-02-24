using Microsoft.AspNetCore.Http;
using Minibank.Core.Services;
using System.Threading.Tasks;

namespace Minibank.Web.Middlewares
{
    public class UserFriendlyExceptionMiddleware
    {
        public readonly RequestDelegate _next;

        public UserFriendlyExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (UserFriendlyException ex)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Message = ex.Message });
            }
        }
    }
}
