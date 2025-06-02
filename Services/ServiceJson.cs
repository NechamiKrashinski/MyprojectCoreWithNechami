using System.Text.Json;
using project.Interfaces;
using project.Models;

namespace project.Services;


public static class ServiceUtilities
{
    public static void AddService(this IServiceCollection services)
    {
        services.AddScoped<IUserService<Author>, UserServiceJson<Author>>();
        services.AddScoped<IItemService<Book>, ItemServiceJson<Book>>();
      //  services.AddScoped<IAuthentication, AuthenticationService<Author>();
        services.AddScoped<LoginService<Author>>();
    }
}
