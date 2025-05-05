using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

using project.Interfaces;

namespace project.Services;


public class LoginService<T> :ILogin<T> where T : IGeneric, IRole
{
  private readonly IAuthentication<T> authentication;

    public LoginService(IAuthentication<T> service)
    {
        this.authentication = service;
    }


    public string Login(int userId)
{
    System.Console.WriteLine(userId + " id in login service");
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
            // new("Name", CurrentUser.Name),
            new("Id", userId.ToString()),
            new(ClaimTypes.Role, CurrentUser.role.ToString()),
             new(ClaimTypes.Name, CurrentUser.Name.ToString())
        };
        var token = TokenService.GetToken(claims);
        string tokenString = TokenService.WriteToken(token);
            Console.WriteLine(tokenString);
        return tokenString;
    }
}

    
}
   
