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
        Console.WriteLine("Checking AuthToken...");

        // בדוק אם הטוקן קיים ואם הוא תקף
        if (string.IsNullOrEmpty(token) || !TokenService.IsTokenValid(token))
        {
            Console.WriteLine("AuthToken is either null or invalid.");

            // בדוק אם הבקשה היא לדף הכניסה
            if (context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Request is for login page, proceeding...");
                await _next(context);
                return;
            }

            // אם הטוקן לא תקף, הפנה לדף הכניסה
            Console.WriteLine("Redirecting to login page...");
            context.Response.Redirect("/login.html");
            return;
        }
        else
        {
            Console.WriteLine("AuthToken is valid, decoding...");

            // קריאה לפונקציה SaveToken
            var claims = TokenService.DecodeToken(token);
            if (claims == null)
            {
                Console.WriteLine("Claims decoding failed, redirecting to login...");
                context.Response.Redirect("/login.html");
                return;
            }

            int userIdClaim = -1;
            Role roleClaim = Role.Reader;

            try
            {
                userIdClaim = int.Parse(claims.FindFirst("Id")?.Value);
                roleClaim = (Role)Enum.Parse(typeof(Role), claims.FindFirst("Role")?.Value);
                Console.WriteLine($"User ID: {userIdClaim}, Role: {roleClaim}");

                CurrentUser.SetCurrentUser(userIdClaim, roleClaim);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה במקרה של ערך לא תקין
                Console.WriteLine($"Error parsing claims: {ex.Message}");
                context.Response.Redirect("/login");
                return;
            }
        }

        Console.WriteLine("Proceeding with the request...");
        await _next(context);
        System.Console.WriteLine("Request processing completed.");
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
