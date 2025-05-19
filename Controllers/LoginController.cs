
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
namespace project.Controllers;
[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{


    private readonly ILogin<Author> loginService;
 


    public LoginController(ILogin<Author> loginService)
    {      
        this.loginService = loginService;
    }



    [HttpPost]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
    {
        
        string token = loginService.Login(loginRequest.password,loginRequest.name);
        if (token == "Invalid credentials")
        {
            return BadRequest("Invalid credentials");
        }
        else
        {
            System.Console.WriteLine(token);
                     //Save token in a cookie
            HttpContext.Response.Cookies.Append("authToken", token, new CookieOptions
            {
                HttpOnly = false,
                Secure = false, // Set to true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
         

            
            
         return Ok(new { token  });
        }
        // }
        //      return Ok(new { token });
    }
    [HttpDelete]
public IActionResult Logout()
{

    HttpContext.Response.Cookies.Delete("authToken");

    
    return Ok("User logged out successfully.");
}
    public class LoginRequest
    {
        public string password { get; set; }
        public string name { get; set; } // Added the missing 'name' property
    }
}