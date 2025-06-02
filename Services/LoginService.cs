using System.Security.Claims;
using project.Interfaces;

namespace project.Services;

public class LoginService<T>
    where T : IUser
{
    private readonly IUserService<T> userService;

    public LoginService(IUserService<T> userService)
    {
        this.userService = userService;
    }

    public string Login(string email, string password)
    {
        userService.isAuth = true;
        var userAthenticate = userService
            .Get()
            .FirstOrDefault(a => a.email == email && a.password == password);
        userService.isAuth = false;

        if (userAthenticate == null)
            return "User not found";
        var claims = new List<Claim>
        {
            new("Id", userAthenticate.Id.ToString()),
            new("Role", userAthenticate.role.ToString()),
        };
        var token = TokenService.GetToken(claims);
        var tokenString = TokenService.WriteToken(token);
        return tokenString;
    }
}
