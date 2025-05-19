
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace project.middlewares;
public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseUserMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserMiddleware>();
    }
}

public class UserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CurrentUserService currentUserService;

    public UserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        var isLoginRequest = context.Request.Path.Value.Equals("/login", StringComparison.OrdinalIgnoreCase);
        
      

        if (!isLoginRequest)
        {
            var token = context.Request.Cookies["authToken"];
            //var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

            if (string.IsNullOrEmpty(token))
            {
                // System.Console.WriteLine("Token is missing or empty");
                //context.Response.Redirect("/login.html"); // הפנה לעמוד הלוגין
                //context.Response.StatusCode = StatusCodes.Status401Unauthorized; // מחזיר שגיאה
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            
            var currentUser = serviceProvider.GetRequiredService<CurrentUserService>();
            currentUser.Id = int.Parse(jwtToken.Claims.First(claim => claim.Type == "Id").Value);
            currentUser.Name = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            currentUser.Role = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
        // System.Console.WriteLine(currentUser.Name + " user in user middleware");
        }

        // קריאה ל-Middleware הבא
        await _next(context);
    }
}
