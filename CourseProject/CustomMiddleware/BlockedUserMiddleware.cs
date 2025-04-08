using CourseProject.Data.Repositories;
using CourseProject.Services;
using Enums;
using Microsoft.AspNetCore.Authentication;

namespace CourseProject.CustomMiddleware
{
    public class BlockedUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public BlockedUserMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path.StartsWith("/Auth/Login") || path.StartsWith("/Auth/Logout"))
            {
                await _next(context);
                return;
            }

            if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                if (context.Request.Method != HttpMethods.Get)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
                        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepositoryReal>();

                        var userId = authService.GetUserId();

                        if (userId is not null)
                        {
                            var user = userRepository.GetById(userId.Value);

                            if (user is null || user.IsBlocked || !user.Role.HasFlag(Role.Admin))
                            {
                                await context.SignOutAsync(AuthService.AUTH_TYPE_KEY);
                                context.Response.Redirect("/Auth/Login");
                                return;
                            }
                        }
                        else
                        {
                            await context.SignOutAsync(AuthService.AUTH_TYPE_KEY);
                            context.Response.Redirect("/Auth/Login");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
