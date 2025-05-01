<<<<<<< HEAD
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using project.Services;
=======
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using project.Services;

namespace Services;
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa

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
<<<<<<< HEAD
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
                options.LoginPath = "/login"; // נתיב לדף ההתחברות
                options.AccessDeniedPath = "/access-denied"; // נתיב לדף שגיאה אם אין הרשאה
=======
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(configuration =>
            {
                configuration.RequireHttpsMetadata = false;
                configuration.TokenValidationParameters =
                    TokenService.GetTokenValidationParameters();
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
            });

        services.AddAuthorization(cfg =>
        {
            cfg.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
            cfg.AddPolicy("Author", policy => policy.RequireClaim("Role", "Author", "Admin"));
<<<<<<< HEAD
             cfg.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
        });


=======
        });
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
        return services;
    }
}
