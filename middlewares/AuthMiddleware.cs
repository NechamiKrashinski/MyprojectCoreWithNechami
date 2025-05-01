using System.Security.Claims;
using project.Controllers;
using project.Interfaces;
using project.Models;
using project.Services;

namespace project.middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Cookies["AuthToken"];

        // בדוק אם הטוקן קיים ואם הוא תקף
        if (string.IsNullOrEmpty(token) || !TokenService.IsTokenValid(token))
        {
            // בדוק אם הבקשה היא לדף הכניסה
            if (context.Request.Path.Value.Equals("/login", StringComparison.OrdinalIgnoreCase))
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
            // אם אתה רוצה להחזיר את ה-claims כתגובה, תוכל להחזיר את זה כאן
            //login.SaveToken(claims);
            // אם אתה רוצה להחזיר את ה-claims כתגובה, תוכל להחזיר את זה כאן
            // context.Response.WriteAsync(claims.ToString());
            // הפנה לדף /author
            context.Response.Redirect("/author.html");
        }
        // המשך לעבד את הבקשה
        await _next(context);
    }
}
public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}
