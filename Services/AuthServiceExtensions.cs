using project.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Services;

public static class AuthServiceExtensions
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
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
                    cfg.AddPolicy("Admin",
                    policy => policy.RequireClaim("type", "Admin"));
                    cfg.AddPolicy("Auther",
                    policy => policy.RequireClaim("type", "Auther", "Admin"));

                });
        return services;


    }
}