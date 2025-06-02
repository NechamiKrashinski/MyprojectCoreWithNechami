using System.Text.Json;
using project.Interfaces;
using project.Models;

namespace project.Services;

public static class ServiceUtilities
{
    public static void AddService(this IServiceCollection services)
    {
        services.AddScoped<IUserService<Author>, UserServiceJson<Author, Book>>();
        services.AddScoped<IItemService<Book>, ItemServiceJson<Book>>();
        services.AddScoped<LoginService<Author>>();
        services.AddScoped<Lazy<IItemService<Book>>>(provider => new Lazy<IItemService<Book>>(() =>
            provider.GetRequiredService<IItemService<Book>>()
        ));
    }
}
