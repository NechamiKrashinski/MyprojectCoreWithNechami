using Microsoft.AspNetCore.Mvc;

using project.Interfaces;
using project.Models;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;

namespace project.Services;

public abstract class ServiceJson<T> : IService<T> where T : IGeneric
{
    protected List<T> MyList { get; }
    protected static string fileName;
    private string filePath;

    public ServiceJson(IHostEnvironment env, string fName)
    {
        fileName = fName;
        filePath = Path.Combine(env.ContentRootPath, "data", fileName);
        if (!File.Exists(filePath))
        {
            System.Console.WriteLine("--------------------------------------------------");
            MyList = new List<T>(); // או טיפול אחר במקרה שהקובץ לא קיים
            return;

        }

        using (var jsonFile = File.OpenText(filePath))
        {
            System.Console.WriteLine("////////////////////////////////////////////////");

            MyList = JsonSerializer.Deserialize<List<T>>(jsonFile.ReadToEnd(),
                    new JsonSerializerOptions
                {
                  PropertyNameCaseInsensitive = true
                }) ?? new List<T>(); ;
            System.Console.WriteLine(MyList.ToString()+"-------------------------------------------------------------------");

        }
    }

    protected void saveToFile()
    {
        System.Console.WriteLine("in save to file----------------------------" + filePath);
        File.WriteAllText(filePath, JsonSerializer.Serialize(MyList));
    }



    public List<T> Get()
    {
        return MyList;
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
    public static void AddSservic(this IServiceCollection services)
    {
        services.AddSingleton<IService<Book>, BookServiceJson>();
        services.AddSingleton<IService<User>, UserServiceJson>();
    }
}