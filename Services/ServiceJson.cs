using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using project.Interfaces;
using project.Models;

namespace project.Services;


public abstract class ServiceJson<T> : GetFuncService<T>, IService<T> where T : IGeneric 
{
    public ServiceJson(IHostEnvironment env) : base(env)
    {

    }

    protected void saveToFile()
    {
        System.Console.WriteLine("in save to file----------------------------" + filePath);
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var jsonData = JsonSerializer.Serialize(MyList, options);
        File.WriteAllText(filePath, jsonData);
    }
    



public abstract List<T> Get();
    public abstract T Get(int id);

    public abstract int Insert(T newT);

    public abstract bool Update(int id, T book);

    public abstract bool Delete(int id);
    
}

public static class ServiceUtilities
{
    public static void AddService(this IServiceCollection services)
    {
        
        services.AddScoped<IService<Book>, BookServiceJson>();
        services.AddScoped<IService<Author>, AuthorServiceJson>();
        //  services.AddScoped<IHelpService<Book>, HelpService<Book>>();
        //   services.AddScoped<IHelpService<Author>, HelpService<Author>>();
        services.AddScoped<IService<Author>, AuthorServiceJson>();
        services.AddScoped<IAuthentication<Author>, AuthenticationService<Author>>();
        services.AddScoped<ILogin<Author>, LoginService<Author>>();
       // services.AddScoped<BookServiceJson, BookServiceJson>();
        services.AddScoped<CurrentUserService, CurrentUserService>();
        services.AddScoped<CurrentUserService>();        // services.AddHttpContextAccessor();
      
    }
}