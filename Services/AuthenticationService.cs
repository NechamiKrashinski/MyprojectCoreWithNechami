using project.Interfaces;
using project.Models;

namespace project.Services;

public class AuthenticationService<T> : GetFuncService<T>, IAuthentication<T>
    where T : ILogin<T>
{
    public AuthenticationService(IHostEnvironment env)
        : base(env) { }

    public override List<T> Get()
    {
        System.Console.WriteLine("------------------Get-----------------");
        System.Console.WriteLine(MyList.Count+"==="+MyList.ToString());
        System.Console.WriteLine("------------------Get-----------------");

        return MyList;
    }
}
