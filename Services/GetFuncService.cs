using System.Text.Json;

namespace project.Services;

public class GetFuncService<T>
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

       public List<T> Get()
    {
        return MyList;
    }

}
