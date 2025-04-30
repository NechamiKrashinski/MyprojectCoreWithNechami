// using Microsoft.AspNetCore.Mvc;
// using project.Interfaces;
// using project.Models;
// using project.Services;

// namespace project.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class LoginController2<T> : ControllerBase
//     where T : IGeneric, IRole
// {
//     private readonly LoginService<T> loginService;

//     public LoginController2(LoginService<T> loginService)
//     {
//         this.loginService = loginService;
//     }

// [HttpGet]
//     public ActionResult<IEnumerable<T>> Get()
// {
//     // הנח שאתה יוצר רשימה של T כלשהו
//     var list = new List<T>();
//     // הוסף אובייקטים של T לרשימה
//     return Ok(list);
// }

//     [HttpPost]
//     public ActionResult<Author> Login([FromBody] T user)
//     {
//         string token = loginService.Login(user);
//         if (token == "User not found")
//             return BadRequest("Invalid credentials");
//         return new OkObjectResult(token);
//     }
// }
