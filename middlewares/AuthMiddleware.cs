using System.Security.Claims;
using project.Controllers;
using project.Interfaces;
using project.Models;
using project.Services;

namespace project.middleware;

public class AuthMiddleware<T>
    where T : IUser
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        System.Console.WriteLine("auth middlware 1");
        var token = context.Request.Cookies["AuthToken"];

        // בדוק אם הטוקן קיים ואם הוא תקף
        if (string.IsNullOrEmpty(token) || !TokenService.IsTokenValid(token))
        {
            // בדוק אם הבקשה היא לדף הכניסה
            if (context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // אם הטוקן לא תקף, הפנה לדף הכניסה
            context.Response.Redirect("/login.html");
            return;
        }
        else
        {
            // קריאה לפונקציה SaveToken
            var claims = TokenService.DecodeToken(token);
            if (claims == null)
            {
                context.Response.Redirect("/login.html");
                return;
            }

            var expClaim = claims.FindFirst("exp");
            if (expClaim != null && long.TryParse(expClaim.Value, out long exp))
            {
                bool isValid = exp > DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                Console.WriteLine(
                    $"Token is valid: {isValid}, Expiration: {DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime}"
                );
            }

            int userIdClaim = -1;
            Role roleClaim = Role.Reader;

            try
            {
                if (!Enum.TryParse<Role>(claims.FindFirst("Role")?.Value, out roleClaim))
                {
                    throw new ArgumentException("Invalid role claim");
                }

                if (!int.TryParse(claims.FindFirst("Id")?.Value, out userIdClaim))
                {
                    throw new ArgumentException("Invalid user ID claim");
                }

                CurrentUser.SetCurrentUser(userIdClaim, roleClaim);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה במקרה של ערך לא תקין
                context.Response.Redirect("/login");
                return;
            }
        }

        await _next(context);
    }

    // המשך לעבד את הבקשה
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseAuthMiddleware<T>(this IApplicationBuilder builder)
        where T : IUser
    {
        return builder.UseMiddleware<AuthMiddleware<T>>();
    }
}
