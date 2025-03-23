using Microsoft.AspNetCore.Mvc;

using project.Interfaces;
using project.Models;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;
namespace project.Services;

public abstract class ServiceJson<T> : IService
{
    List<T> MyList {get;}
    protected static string fileName;
    private string filePath;

    public ServiceJson (IHostEnvironment env)
    {
      filePath=Path.Combine(env.ContentRootPath,"data",fileName);
      using(var jsonFile = File.OpenText(filePath)){
          MyList=JsonSerializer.Deserialize<List<T>>(jsonFile.ReadToEnd(),
          new JsonSerializerOptions{
              PropertyNameCaseInsensitive = true
          });
      }
    }

private void saveToFile(){
    File.WriteAllText(filePath,JsonSerializer.Serialize(MyList));
}



    public  List<T> Get(){
        return MyList;
    }

     public  T Get(int id){
        var book= MyList.FirstOrDefault(b=> b.Id==id);
        return book;
    }

 public  abstract Insert(T newT);

    public abstract bool Update(int id ,T book);
   

    public bool Delete(int id)
    {
        var currentT= MyList.FirstOrDefault(b=> b.Id==id);
        if(currentT == null)
            return false;
        
        int index = MyList.IndexOf(currentT);
        MyList.RemoveAt(index);
        saveToFile();
        return true;
    }

}