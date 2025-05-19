
using System.IdentityModel.Tokens.Jwt;
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

            var currentUser = serviceProvider.GetRequiredService<CurrentUserService>();
            currentUser.Id = int.Parse(claims.FindFirst(c => c.Type == "Id").Value);
            currentUser.Role = claims.FindFirst(c => c.Type == ClaimTypes.Role).Value;
            currentUser.Name = claims.FindFirst(c => c.Type == ClaimTypes.Name).Value;

             
            
            
           
        }
        // המשך לעבד את הבקשה
        await _next(context);
    }
    //     private bool IsTokenValid(string token)
    // {
    //     System.Console.WriteLine(""+TokenService.ValidateToken(token) != null + " token is valid");
    //     // לוגיקה לבדוק אם התוקן תקף
    //     return TokenService.ValidateToken(token) != null;
    // }
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder) 
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}

// using project.Services;

// namespace project.middlewares;
// public static partial class MiddlewareExtensions
// {
//     public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
//     {
//         return builder.UseMiddleware<AuthMiddleware>();
//     }
// }

// public class AuthMiddleware
// {
//     private readonly RequestDelegate _next;

//     public AuthMiddleware(RequestDelegate next)
//     {
//         _next = next;
//     }

//     public async Task Invoke(HttpContext context)
//     {
//         var isLoginRequest = context.Request.Path.Value.Equals("/login", StringComparison.OrdinalIgnoreCase);
//        var token = context.Request.Cookies["authToken"];

//         // הדפסת כל הכותרות
//         // foreach (var header in context.Request.Headers)
//         // {
//         //     Console.WriteLine($"{header.Key}: {header.Value}");
//         // }

       
//         Console.WriteLine($"Request Path: {context.Request.Path.Value}");
//         Console.WriteLine($"Is Login Request: {isLoginRequest}");

//         if (!isLoginRequest)
//         {
//             if (string.IsNullOrEmpty(token) || !IsTokenValid(token))
//             {
//                 Console.WriteLine("Token is invalid or missing, redirecting to login.");
//                 context.Response.Redirect("/login.html", true);
//                 return; // יוצא מהמידלואר לאחר ההפנייה
//             }
//             else
//             {
//                 Console.WriteLine("Token is valid, proceeding to next middleware.");
//                 await _next(context);
//             }
//         }
//         else
//         {
//             Console.WriteLine("Login request, proceeding to next middleware.");
//             await _next(context);
//         }
//     }


   

//     private bool IsTokenValid(string token)
//     {
//         System.Console.WriteLine(""+TokenService.ValidateToken(token) != null + " token is valid");
//         // לוגיקה לבדוק אם התוקן תקף
//         return TokenService.ValidateToken(token) != null;
//     }
// }