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

    // protected void saveToFile()
    // {
    //     System.Console.WriteLine("in save to file----------------------------" + filePath);
    //     File.WriteAllText(filePath, JsonSerializer.Serialize(MyList));
    // }
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




    public abstract T Get(int id);

    public abstract int Insert(T newT);

    public abstract bool Update(int id, T book);

    public bool Delete(int id)
    {
        var currentT = MyList.FirstOrDefault(b => b.Id == id);
        if (currentT == null)
            return false;

        int index = MyList.IndexOf(currentT);
        MyList.RemoveAt(index);
        saveToFile();
        return true;
    }
}

public static class ServiceUtilities
{
    public static void AddService(this IServiceCollection services)
    {
        System.Console.WriteLine("AddService----------------------------");
        services.AddScoped<IService<Book>, BookServiceJson>();
        services.AddScoped<IService<Author>, AuthorServiceJson>();
        services.AddScoped<IAuthentication<Author>, AuthenticationService<Author>>();
        services.AddScoped<ILogin<Author>, LoginService<Author>>();
        services.AddScoped<BookServiceJson, BookServiceJson>();
        services.AddScoped<CurrentUserService, CurrentUserService>();
        services.AddScoped<CurrentUserService>();        // services.AddHttpContextAccessor();
        System.Console.WriteLine("finish to load services");
    }
}