using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using project.Services;



public static class AuthServiceExtensions
{
    public static IServiceCollection AddCustomAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // הוסף את מנגנון ה-Cookie
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // קבלת התוקן מהקוקי
                        if (context.Request.Cookies.ContainsKey("authToken"))
                        {
                            context.Token = context.Request.Cookies["authToken"];
                            System.Console.WriteLine("Token is heare in authServiceExtensions");

                        }
                        return Task.CompletedTask;
                    }
                };
                options.RequireHttpsMetadata = false; // בהתאם לצורך שלך
                options.TokenValidationParameters = TokenService.GetTokenValidationParameters();
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
               options.LoginPath = "/login.html"; // נתיב לדף ההתחברות
                options.AccessDeniedPath = "/access-denied"; // נתיב לדף שגיאה אם אין הרשאה
            });
        services.AddAuthorization(cfg =>
        {
            cfg.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
            cfg.AddPolicy("Author", policy => policy.RequireClaim(ClaimTypes.Role, "Author", "Admin"));

             cfg.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
        });



        return services;
    }
}


// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using project.Services;

// public static class AuthServiceExtensions
// {
//     public static IServiceCollection AddCustomAuthentication(
//         this IServiceCollection services,
//         IConfiguration configuration
//     )
//     {
//         services
//             .AddAuthentication(options =>
//             {
//                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//                 options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // הוסף את מנגנון ה-Cookie
//             })
//             .AddJwtBearer(options =>
//             {
//                 options.Events = new JwtBearerEvents
//                 {
//                     OnMessageReceived = context =>
//                     {
//                         // קבלת התוקן מהקוקי
//                         if (context.Request.Cookies.ContainsKey("authToken"))
//                         {
//                             context.Token = context.Request.Cookies["authToken"];
//                             System.Console.WriteLine("Token is here in authServiceExtensions");
//                         }

//                         // בדוק אם הבקשה היא לעמוד הלוגין
//                         if (context.HttpContext.Request.Path.StartsWithSegments("/login.html"))
//                         {
//                             // אם הבקשה היא לעמוד הלוגין, אין צורך בטוקן
//                             return Task.CompletedTask;
//                         }

//                         return Task.CompletedTask;
//                     },
//                     OnTokenValidated = context =>
//                     {
//                         // בדוק אם התוקן תקף
//                         if (context.SecurityToken is not JwtSecurityToken jwtToken || 
//                             !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
//                         {
//                             System.Console.WriteLine("Invalid token");

//                             context.Fail("Invalid token");
//                             context.Response.Redirect("/login.html"); // הפנה לעמוד הלוגין
//                             return Task.CompletedTask;
//                         }

//                         return Task.CompletedTask;
//                     },
//                     OnAuthenticationFailed = context =>
//                     {
//                         // במקרה של שגיאה באימות הטוקן
//                         if (context.Exception is SecurityTokenExpiredException)
//                         {
//                             System.Console.WriteLine("Token has expired");

//                             context.Response.Redirect("/login.html");
//                             context.Response.StatusCode = 401; // סטטוס קוד לא מורשה
//                             // context.HandleResponse(); // מונע את המשך העיבוד של הבקשה
//                         }
//                         return Task.CompletedTask;
//                     }
//                 };
//                 options.RequireHttpsMetadata = false; // בהתאם לצורך שלך
//                 options.TokenValidationParameters = TokenService.GetTokenValidationParameters();
//             })
//             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//             {
//                 options.LoginPath = "/login.html"; // נתיב לדף ההתחברות
//                 options.AccessDeniedPath = "/access-denied"; // נתיב לדף שגיאה אם אין הרשאה
//             });

//         services.AddAuthorization(cfg =>
//         {
//             cfg.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
//             cfg.AddPolicy("Author", policy => policy.RequireClaim(ClaimTypes.Role, "Author", "Admin"));
//         });

//         return services;
//     }
// }