using WebAppSalesManagement.Services;

namespace WebAppSalesManagement.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApiService apiService)
        {
            var token = context.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(token))
            {
                apiService.SetAuthToken(token);
            }

            await _next(context);
        }
    }

    public static class TokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>();
        }
    }
}