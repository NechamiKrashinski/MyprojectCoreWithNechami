using System.Security.Claims;
using project.Interfaces;

namespace project.Services;

public class LoginService<T> : ILogin<T>
    where T : IGeneric, IRole
{
    private readonly IAuthentication<T> authenticationService;

    public LoginService(IAuthentication<T> authentication)
    {
        this.authenticationService = authentication;
    }

    public string Login(int id)
    {
        var userAthenticate = authenticationService.Get().FirstOrDefault(a => a.Id == id);

        if (userAthenticate == null)
            return "User not found";

        var claims = new List<Claim>
        {
            new("Id", userAthenticate.Id.ToString()),
            new("Role", userAthenticate.role.ToString()),
        };
        var token = TokenService.GetToken(claims);
        var tokenString = TokenService.WriteToken(token);
        TokenService.SaveToken(tokenString);

        return tokenString;
    }
}
