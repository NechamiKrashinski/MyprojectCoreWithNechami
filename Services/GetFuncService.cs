using System.Text.Json;
using project.Models;

namespace project.Services;

public abstract class GetFuncService<T>
{
    protected List<T> MyList { get; }
    protected static string fileName;
    protected string filePath;

    public GetFuncService(IHostEnvironment env)
    {
        if (typeof(T) == typeof(CurrentUser))
            fileName = "author.json";
        else
            fileName = typeof(T).Name.ToLower() + ".json";
        filePath = Path.Combine(env.ContentRootPath, "data", fileName);
        if (!File.Exists(filePath))
        {
            MyList = new List<T>();
            return;
        }

        using (var jsonFile = File.OpenText(filePath))
        {
            MyList =
                JsonSerializer.Deserialize<List<T>>(
                    jsonFile.ReadToEnd(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new List<T>();
        }
    }

    public void saveToFile()
    {
        var jsonString = JsonSerializer.Serialize(
            MyList,
            new JsonSerializerOptions { WriteIndented = true }
        );
        File.WriteAllText(filePath, jsonString);
    }

    public abstract List<T> Get();
}
