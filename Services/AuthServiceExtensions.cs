
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace project.Services;
public static class AuthServiceExtensions
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(options=>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg=>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.TokenValidationParameters =
                     TokenService.GetTokenValidationParameters();
            });
            
            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("Admin",
                    policy => policy.RequireClaim("type","Admin")); 
                cfg.AddPolicy("User",
                    policy => policy.RequireClaim("type","User","Admin"));
            });

            return services;
    }
}