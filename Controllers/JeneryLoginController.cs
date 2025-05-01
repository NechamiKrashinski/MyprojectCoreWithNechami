// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using project.Interfaces;
// using project.Models;
// using project.Services;
// using System.Collections.Generic;
// using System.Security.Claims;
// namespace project.Controllers;
// [ApiController]
// [Route("[controller]")]
// public class JeneryLoginController<T> : ControllerBase where T : IGeneric, IRole
// {

   
//      private readonly ILogin<T> loginService;


//     public JeneryLoginController(ILogin<T> loginService)
//     {
      
//         this.loginService = loginService;
//     }
    


   
//     [HttpPost]
//     public ActionResult<string> Login(int id)
//     {
//       string token = loginService.Login(id);
//         if (token == "Invalid credentials")
//         {
//             return BadRequest("Invalid credentials");
//         }
//         else
//         {
//             return Ok(token);
//         }

//     }
// }