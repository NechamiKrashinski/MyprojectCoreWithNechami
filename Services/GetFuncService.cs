using System.Text.Json;
using project.Interfaces;
using project.Models;
namespace project.Services;
public abstract class GetFuncService<T> where T:IGeneric

{
    protected List<T> MyList { get; }
    protected static string fileName;
    protected string filePath;

    public GetFuncService(IHostEnvironment env)
    {
        fileName = typeof(T).Name.ToLower() + ".json";
        filePath = Path.Combine(env.ContentRootPath, "data", fileName);
        if (!File.Exists(filePath))
        {

           
            MyList = new List<T>(); // או טיפול אחר במקרה שהקובץ לא קיים
            return;


        }

        using (var jsonFile = File.OpenText(filePath))
        {

            

            MyList = JsonSerializer.Deserialize<List<T>>(jsonFile.ReadToEnd(),
                    new JsonSerializerOptions
                {
                  PropertyNameCaseInsensitive = true
                }) ?? new List<T>(); ;
            System.Console.WriteLine(MyList.ToString()+"--------------------------");

        }
    }

    



    public abstract List<T> Get();


}
