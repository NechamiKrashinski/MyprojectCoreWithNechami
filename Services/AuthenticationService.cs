using project.Models;

namespace project.Services;

public class AuthenticationService : GetFuncService<Auther>
{
    public AuthenticationService(IHostEnvironment env)
        : base(env) { }
}
