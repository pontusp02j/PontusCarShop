using Core.Repositories.Users;

namespace Core.Middleware.Security
{
    public class RestrictedIpMiddleware
    {
        private readonly RequestDelegate _next;
        private const string LoginPath = "/api/login/user";
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RestrictedIpMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var securityRepository = scope.ServiceProvider.GetRequiredService<ISecurityRepository>();

            if (!(context.Request.Path == LoginPath && context.Request.Method == "POST"))
            {
                await _next(context);
                return;
            }

            var ip = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(ip))
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Could not find ip address in request");
                return;
            }

            if(await securityRepository.IsIpAddressRestrictedAsync(ip))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Ip address is restricted");
                return;
            }

            await _next(context);
        }
    }
}
