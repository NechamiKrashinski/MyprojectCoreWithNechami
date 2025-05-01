using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
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




    public T Get(int id)
    {
        var t = MyList.FirstOrDefault(b => b.Id == id);
        return t;
    }

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
        services.AddSingleton<IService<Book>, BookServiceJson>();
        services.AddSingleton<IService<Author>, AuthorServiceJson>();
        services.AddScoped<IAuthentication<Author>, AuthenticationService<Author>>();
        services.AddScoped<ILogin<Author>, LoginService<Author>>();

    }}