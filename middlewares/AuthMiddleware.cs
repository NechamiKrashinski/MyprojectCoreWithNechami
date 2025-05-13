using System.Security.Claims;
using project.Controllers;
using project.Interfaces;
using project.Models;
using project.Services;

namespace project.middleware;

public class AuthMiddleware<T>
    where T : ILogin<T>, new()
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

            var currentUser = new T
            {
                Id = int.Parse(claims.FindFirst(c => c.Type == "Id").Value),
                role = (Role)Enum.Parse(typeof(Role), claims.FindFirst(c => c.Type == "Role").Value),
            };

            var loginService = serviceProvider.GetRequiredService<ILogin<T>>();
            loginService.SetCurrentUser(currentUser);
        }
        // המשך לעבד את הבקשה
        await _next(context);
    }
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseAuthMiddleware<T>(this IApplicationBuilder builder) where T: ILogin<T>, new()
    {
        return builder.UseMiddleware<AuthMiddleware<T>>();
    }
}
