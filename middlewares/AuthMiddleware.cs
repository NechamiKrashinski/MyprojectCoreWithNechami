// namespace project.middlewares;
// public class AuthMiddleware
// {
//     private readonly RequestDelegate _next;

//     public AuthMiddleware(RequestDelegate next)
//     {
//         _next = next;
//     }

//     public async Task Invoke(HttpContext context)
//     {
//         var token = context.Request.Cookies["authToken"];

//         // אם הטוקן לא קיים או לא תקף
//         if (string.IsNullOrEmpty(token) || !IsTokenValid(token))
//         {

//             // נווט לעמוד הכניסה אם המשתמש מנסה לגשת לעמודים אחרים
//             if (!context.Request.Path.StartsWithSegments("/login"))
//             {
//                 System.Console.WriteLine("Token is invalid or missing._________________________________________________________");
//                 context.Response.Redirect("/login");
//                 System.Console.WriteLine("Token is invalid or missing.~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
//                 return;
//             }
//         }
//         else
//         {
//             // אם הטוקן תקף, הוסף אותו לקונטקסט של הבקשה
//             context.Request.Headers["Authorization"] = $"Bearer {token}";
//             System.Console.WriteLine("Token is heare");

//             await _next(context);
//         }



//     }

//     private bool IsTokenValid(string token)
//     {
//         // לוגיקה לבדוק אם הטוקן תקף
//         return true; // החזר true אם הטוקן תקף
//     }
// }
 namespace project.middlewares;
 public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Cookies["authToken"]; // קרא את הטוקן מה-Cookies

        if (string.IsNullOrEmpty(token) || !IsTokenValid(token))
        {
           
           
            if (!context.Request.Path.StartsWithSegments("/login"))
            {
                 System.Console.WriteLine("Token is invalid or missing.");
                context.Response.Redirect("/login");
                return;
            }
        }
         context.Request.Headers["Authorization"] = $"Bearer {token}";
//             System.Console.WriteLine("Token is heare");
        await _next(context);
    }

    private bool IsTokenValid(string token)
    {
        // בדוק אם הטוקן תקף (מימוש מותאם אישית)
        return !string.IsNullOrEmpty(token); // לדוגמה בלבד
    }
}