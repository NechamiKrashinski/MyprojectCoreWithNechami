using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
using project.Services;

namespace project.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
// where T : IGeneric, IRole
{
    private readonly LoginService<Author> loginService;

    public LoginController(LoginService<Author> loginService)
    {
        this.loginService = loginService;
    }

    [HttpPost]
    public ActionResult<Author> Login([FromBody] int id)
    {
        string token = loginService.Login(id);
        if (token == "User not found")
            return BadRequest("Invalid credentials");
        return new OkObjectResult(token);
    }
}
