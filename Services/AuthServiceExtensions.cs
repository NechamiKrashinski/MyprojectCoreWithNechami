using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using project.Services;

namespace Services;

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
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(configuration =>
            {
                configuration.RequireHttpsMetadata = false;
                configuration.TokenValidationParameters =
                    TokenService.GetTokenValidationParameters();
            });

        services.AddAuthorization(cfg =>
        {
            cfg.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
            cfg.AddPolicy("Author", policy => policy.RequireClaim("Role", "Author", "Admin"));
        });
        return services;
    }
}
