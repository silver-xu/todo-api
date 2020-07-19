using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Todo.Services.Interfaces;

namespace Todo.API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            if (context.Request.Headers["X-API-Key"].Any())
            {
                var headerKey = context.Request.Headers["X-API-Key"].FirstOrDefault();
                await ValidateApiKey(authService, context, headerKey);
            }
            else
            {
                await Invalidate(context);
            }
        }

        private async Task ValidateApiKey(IAuthService authService, HttpContext context, string token)
        {
            var userId = await authService.GetUserIdByToken(token);

            if (userId == null)
            {
                await Invalidate(context);
            }
            else
            {
                var identity = new GenericIdentity(userId.Value.ToString());
                var principal = new GenericPrincipal(identity, new[] { "ApiUser" });

                context.User = principal;
                await _next(context);
            }
        }

        private async Task Invalidate(HttpContext context)
        {
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid API Key");
            }
        }
    }
}
