using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DevSkill.Inventory.Web.Middlewares
{
    public class UserStatusMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserStatusMiddleware> _logger;

        public UserStatusMiddleware(RequestDelegate next, ILogger<UserStatusMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                try
                {
                    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var user = await userManager.FindByIdAsync(userId);
                        if (user != null && user.Status == Status.Inactive)
                        {
                            _logger.LogWarning("Inactive user {UserId} attempted to access the application", userId);
                            await signInManager.SignOutAsync();
                            context.Response.Redirect("/Account/AccessDenied");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking user status in middleware");
                }
            }

            await _next(context);
        }
    }

    public static class UserStatusMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserStatusCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserStatusMiddleware>();
        }
    }
}