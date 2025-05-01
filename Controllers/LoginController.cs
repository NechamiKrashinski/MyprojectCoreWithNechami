using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using project.Interfaces;
using project.Models;
using project.Services;
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
        System.Console.WriteLine(loginRequest.id + " controller");
        string token = loginService.Login(loginRequest.id);
        if (token == "Invalid credentials")
        {
            return BadRequest("Invalid credentials");
        }
        else
        {
            System.Console.WriteLine(token);
            // Save token in a cookie
            HttpContext.Response.Cookies.Append("authToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
            System.Console.WriteLine(" controller 3");
            var handler = new JwtSecurityTokenHandler();
            System.Console.WriteLine(" controller 4");
            var jwtToken = handler.ReadJwtToken(token); // token הוא הטוקן שקיבלת
            System.Console.WriteLine(" controller 5");
            var userRole = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            System.Console.WriteLine(userRole + " userRole controller");
            var claims = new List<Claim>
        {
            new Claim("Id", loginRequest.id.ToString()) ,
            new Claim(ClaimTypes.Role, userRole) // הוסף את סוג המשתמש
        };
            System.Console.WriteLine(claims + " claims controller");
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // הכנס את ה-ClaimsPrincipal להקשר של המשתמש
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            System.Console.WriteLine("  controller 6");
            var userClaims = HttpContext.User.Claims;
               System.Console.WriteLine(userClaims+ "  userClaims");
            foreach (var claim in userClaims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
            System.Console.WriteLine("  controller 7");

        

    try
    {
        // מנסה לאמת את הטוקן
        ClaimsPrincipal principal = TokenService.ValidateToken(token);
        
        // אם ההגעה כאן, האימות הצליח
        Console.WriteLine(" // אם ההגעה כאן, האימות הצליחToken is valid.");
    }
    catch (SecurityTokenException)
    {
        // אם הושלך חריגה, האימות נכשל
        Console.WriteLine("   // אם הושלך חריגה, האימות נכשלToken is invalid.");
    }
    catch (Exception ex)
    {
        // טיפול בשגיאות אחרות אם יש
        Console.WriteLine($"An error occurred: {ex.Message}");
    }

            return Ok(token);
        }
    }
    public class LoginRequest
    {
        public int id { get; set; }
    }
}
