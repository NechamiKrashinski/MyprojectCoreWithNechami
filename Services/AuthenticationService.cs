using System.Text.Json;
using project.Interfaces;
using project.Models;
namespace project.Services;
public class AuthenticationService<T> : GetFuncService<T>,IAuthentication<T> where T : IGeneric ,IRole
{
    public AuthenticationService(IHostEnvironment env) : base(env)
    {


    }
    public override List<T> Get()
    {
        return MyList;
    }

   
}


