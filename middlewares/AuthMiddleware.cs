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
        var token = context.Request.Cookies["AuthToken"];
        // בדוק אם הטוקן קיים ואם הוא תקף
        if (string.IsNullOrEmpty(token) || !TokenService.IsTokenValid(token))
        {
            // בדוק אם הבקשה היא לדף הכניסה
            if (context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase))
            {
                // אם הבקשה היא לדף הכניסה, המשך לעבד את הבקשה
                await _next(context);
                return;
            }

            // אם הטוקן לא תקף, הפנה לדף הכניסה
            context.Response.Redirect("/login");
            return;
        }
        else
        {
            // קריאה לפונקציה SaveToken

            var claims = TokenService.DecodeToken(token);
            if (claims == null)
            {
                context.Response.Redirect("/login");
                return;
            }
            int userIdClaim = -1;
            Role roleClaim = Role.Reader;
            // גישה ל-claims דרך המאפיין Claims
            // foreach (var claim in claims.Claims)
            // {
            //     if (claim.Type == "Id")
            //     {
            //         userIdClaim = int.Parse(claim.Value);
            //     }
            //     else if (claim.Type == "Role")
            //     {
            //         roleClaim = (Role)Enum.Parse(typeof(Role), claim.Value);
            //     }
            // }
            userIdClaim = int.Parse(claims.FindFirst("Id")?.Value);
            roleClaim = (Role)Enum.Parse(typeof(Role), claims.FindFirst("Role")?.Value);

            try
            {
                CurrentUser.SetCurrentUser(userIdClaim, roleClaim);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה במקרה של ערך לא תקין
                System.Console.WriteLine($"Error parsing claims: {ex.Message}");
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
