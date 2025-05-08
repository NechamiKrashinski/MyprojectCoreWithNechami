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
    private readonly LoginService<CurrentUser> loginService;

    public LoginController(LoginService<CurrentUser> loginService)
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
                HttpOnly = false,
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
}
