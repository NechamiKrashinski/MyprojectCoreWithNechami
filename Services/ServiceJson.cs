using System.Text.Json;
using project.Interfaces;
using project.Models;

namespace project.Services;

public abstract class ServiceJson<T> : GetFuncService<T>, IService<T>
    where T : IGeneric
{
    protected UserAuth userauth;
    private string _token;

    public string Token
    {
        get => _token;
        set
        {
            Console.WriteLine("Token set to: " + value);
            _token = value;
            userauth = TokenService.GetUserAuth(_token);
            Console.WriteLine("Token set to: " + userauth.ToString());
            System.Console.WriteLine(
                userauth.Id + "===" + userauth.role.ToString() + "====" + userauth.role
            );
        }
    }

    public ServiceJson(IHostEnvironment env)
        : base(env) { }

    protected void saveToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(MyList));
    }

    public override List<T> Get()
    {
        if (userauth.role == Role.Author)
        {
            return MyList.Where(a => userauth.Id == a.Id).ToList();
        }
        else if (userauth.role == Role.Admin)
        {
            return MyList;
        }
        return new List<T>();
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
        services.AddSingleton<IService<Book>, BookServiceJson>();
        services.AddSingleton<IService<Author>, UserServiceJson>();
        services.AddScoped<IAuthentication<UserAuth>, AuthenticationService<UserAuth>>(); // ודא שזה קיים
        services.AddScoped<LoginService<UserAuth>>(); // הוסף שורה זו
        services.AddScoped<ILogin<UserAuth>, LoginService<UserAuth>>();
    }
}
