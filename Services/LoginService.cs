using System.Security.Claims;
<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
=======
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
using project.Interfaces;

namespace project.Services;

<<<<<<< HEAD
public class LoginService<T> :ILogin<T> where T : IGeneric, IRole
{
  private readonly IAuthentication<T> authentication;

    public LoginService(IAuthentication<T> service)
    {
        this.authentication = service;
    }


    public string Login(int userId)
{
    List<T> users = authentication.Get();
    T CurrentUser = users.FirstOrDefault(a => a.Id == userId) ;
    if (users.FirstOrDefault(a => a.Id == userId) == null)
    {
        return "Invalid credentials";
    }
    else
    {
        var claims = new List<Claim>
        {
            new("Id", userId.ToString()),
            new(ClaimTypes.Role, CurrentUser.role.ToString())
        };
        var token = TokenService.GetToken(claims);
        string tokenString = TokenService.WriteToken(token);
            Console.WriteLine(tokenString);
        return tokenString;
    }
}

    
}
   
=======
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
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
