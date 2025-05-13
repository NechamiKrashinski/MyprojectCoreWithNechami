using System.Text.Json;
using project.Interfaces;
using project.Models;

namespace project.Services;

// public abstract class ServiceJson<T> : GetFuncService<T>, IService<T>
//     where T : IGeneric
// {
//     protected CurrentUser currentUser;
//     private string _token;

//     public string Token
//     {
//         get => _token;
//         set
//         {
//             Console.WriteLine("Token set to: " + value);
//             _token = value;
//             currentUser = TokenService.GetCurrentUser(_token);
//             Console.WriteLine("Token set to: " + currentUser.ToString());
//             System.Console.WriteLine(
//                 currentUser.Id + "===" + currentUser.role.ToString() + "====" + currentUser.role
//             );
//         }
//     }

//     public ServiceJson(IHostEnvironment env)
//         : base(env) { }

//     protected void saveToFile()
//     {
//         File.WriteAllText(filePath, JsonSerializer.Serialize(MyList));
//     }

//     public override List<T> Get()
//     {
//         if (currentUser.role == Role.Author)
//         {
//             return MyList.Where(a => currentUser.Id == a.Id).ToList();
//         }
//         else if (currentUser.role == Role.Admin)
//         {
//             return MyList;
//         }
//         return new List<T>();
//     }

//     public T Get(int id)
//     {
//         var t = MyList.FirstOrDefault(b => b.Id == id);
//         return t;
//     }

//     public abstract int Insert(T newT);

//     public abstract bool Update(int id, T book);

//     public bool Delete(int id)
//     {
//         var currentT = MyList.FirstOrDefault(b => b.Id == id);
//         if (currentT == null)
//             return false;

//         int index = MyList.IndexOf(currentT);
//         MyList.RemoveAt(index);
//         saveToFile();
//         return true;
//     }
// }

public static class ServiceUtilities
{
    public static void AddService(this IServiceCollection services)
    {
        services.AddScoped<IService<Book>, BookServiceJson>();
        services.AddScoped<IService<Author>, AuthorServiceJson>();
        services.AddScoped<IAuthentication<Author>, AuthenticationService<Author>>(); // ודא שזה קיים
        services.AddScoped<LoginService<Author>>(); // הוסף שורה זו
    }
}
