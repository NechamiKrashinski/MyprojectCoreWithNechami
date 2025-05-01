using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
using project.Services;

namespace project.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginService<UserAuth> loginService;

    public LoginController(LoginService<UserAuth> loginService)
    {
        this.loginService = loginService;
    }

    [HttpPost]
    public ActionResult Login([FromBody] LoginRequest request)
    {
        string token = loginService.Login(request.Id);
        if (token == "User not found")
            return Unauthorized("Invalid credentials");

        HttpContext.Response.Cookies.Append(
            "AuthToken",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // שקול לשנות ל-false בזמן פיתוח
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
            }
        );

        return Ok(token); // החזר תגובה פשוטה של הצלחה
    }

    public class LoginRequest
    {
        public int Id { get; set; }
    }

    // אם אינך משתמש בפונקציה GetToken, שקול להסיר אותה

    // [HttpGet("save-token")]
    // internal IActionResult SaveToken()
    // {
    //     var token = Request.Cookies["AuthToken"]; // שם הקוקי

    //     if (string.IsNullOrEmpty(token))
    //     {
    //         return Unauthorized("Access Denied");
    //     }
    //     var claims = TokenService.DecodeToken(token);
    //     if (claims == null)
    //     {
    //         return Unauthorized("Invalid token");
    //     }
    //     // אם אתה רוצה להחזיר את ה-claims כתגובה, תוכל להחזיר את זה כאן
    //     userauth.Id = int.Parse(claims.FindFirst(c => c.Type == "Id").Value);
    //     userauth.role = (Role)Enum.Parse(typeof(Role), claims.FindFirst(c => c.Type == "Role").Value);
    //     return Ok(userauth); // החזר את ה-claims או את ה-userauth כתגובה
    // }
}
