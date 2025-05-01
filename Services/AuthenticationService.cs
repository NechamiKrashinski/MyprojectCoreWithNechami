<<<<<<< HEAD
using System.Text.Json;
using project.Interfaces;
using project.Models;
namespace project.Services;
public class AuthenticationService<T> : GetFuncService<T>,IAuthentication<T> where T : IGeneric ,IRole
{
    public AuthenticationService(IHostEnvironment env) : base(env)
    {


    }
=======
using project.Interfaces;
using project.Models;

namespace project.Services;

public class AuthenticationService<T> : GetFuncService<T>, IAuthentication<T>
    where T : IGeneric, IRole
{
    public AuthenticationService(IHostEnvironment env)
        : base(env) { }

>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
    public override List<T> Get()
    {
        return MyList;
    }
<<<<<<< HEAD

   
}


=======
}
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
