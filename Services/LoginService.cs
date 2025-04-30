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

    // public string Login(int id)
    // {
    //     var userAthenticate = authenticationService.Get().FirstOrDefault(a => a.Id == id);
    //     System.Console.WriteLine(userAthenticate);

    //     if (userAthenticate == null)
    //         return "User not found";

    //     var claims = new List<Claim>
    //     {
    //         new("Id", userAthenticate.Id.ToString()),
    //         new("Role", userAthenticate.role.ToString()),
    //     };
    //     var token = TokenService.GetToken(claims);
    //     var tokenString = TokenService.WriteToken(token);
    //     TokenService.SaveToken(tokenString);
    //     return tokenString;
    // }

    public string Login(int id)
    {
        Console.WriteLine($"Attempting to log in user with ID: {id}");

        var users = authenticationService.Get();
        Console.WriteLine("Users list: " + string.Join(", ", users.Select(u => u.Id)));

        var userAthenticate = users.FirstOrDefault(a => a.Id == id);
        Console.WriteLine($"User authentication result: {userAthenticate}");

        if (userAthenticate == null)
        {
            Console.WriteLine($"User with ID: {id} not found");
            return "User not found";
        }

        Console.WriteLine($"User found: {userAthenticate.Id}");

        var claims = new List<Claim>
    {
        new("Id", userAthenticate.Id.ToString()),
        new("Role", userAthenticate.role.ToString()),
    };
        var token = TokenService.GetToken(claims);
        var tokenString = TokenService.WriteToken(token);
        TokenService.SaveToken(tokenString);

        Console.WriteLine($"Token generated for user: {userAthenticate.Id}");

        return tokenString;
    }

}
