
using System.Text.Json;
using project.Interfaces;
using project.Models;
namespace project.Services;
public class AuthenticationService<T> : GetFuncService<T>,IAuthentication<T>  where T : IGeneric, IUser  
{
    public AuthenticationService(IHostEnvironment env) : base(env)
    {


    }

    public List<T> Get()
    {
        return MyList;
    }


   
}




