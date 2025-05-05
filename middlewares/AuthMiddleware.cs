
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
    var isLoginRequest = context.Request.Path.Value.Equals("/login", StringComparison.OrdinalIgnoreCase);
    var token = context.Request.Cookies["AuthToken"];

    Console.WriteLine($"Request Path: {context.Request.Path.Value}");
    Console.WriteLine($"Is Login Request: {isLoginRequest}");
    Console.WriteLine($"Token: {token}");

    if (!isLoginRequest)
    {
        if (string.IsNullOrEmpty(token) || !IsTokenValid(token))
        {
            context.Response.Redirect("/login.html", true);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
    }
System.Console.WriteLine("Token is valid or login request, proceeding to next middleware.");
    await _next(context);
}

    private bool IsTokenValid(string token)
    {
        // לוגיקה לבדוק אם התוקן תקף
        return !string.IsNullOrEmpty(token); // לדוגמה בלבד
    }
}