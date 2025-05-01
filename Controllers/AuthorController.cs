using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;

using project.Services;
using System.Collections.Generic;
using System.Security.Claims;
namespace project.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class AuthorController : ControllerBase
// {
//     private readonly IService<Author> service;
//     private readonly string autherRole;

//     public AuthorController(IService<Author> service)
//     {
        
//         this.service = service;
//         autherRole = HttpContext.User.FindFirst("Role")?.Value;
//     }

//     [HttpGet]
//     [Authorize(policy: "Author")]
//     public ActionResult<IEnumerable<Author>> Get()
//     {

//         if (autherRole == "Admin")
//         {
//             return service.Get();
//         }
//         else if (autherRole == "Author")
//         {
//             var id = int.Parse(HttpContext.User.FindFirst("Id")?.Value);
//             return new List<Author> { service.Get(id) };
//         }

//         return BadRequest("Unauthorized access");
//     }

//     [HttpGet("{id}")]
//     [Authorize(policy: "Admin")]
//     public ActionResult<Author> Get(int id)
//     {
//         var auther = service.Get(id);
//         if (auther == null)
//             throw new ApplicationException("Author not found");
//         return auther;
//     }

//     [HttpPost]
//     [Authorize(policy: "Admin")]
//     public ActionResult Post(Author newUser)
//     {
//         var newId = service.Insert(newUser);

//         if (newId == -1)
//         {
//             return BadRequest();
//         }

//         return CreatedAtAction(nameof(Post), new { Id = newId });
//     }

//     [HttpPut("{id}")]
//     [Authorize(policy: "Author")]
//     public ActionResult Put(int id, Author auther)
//     {
//         if (autherRole == "Admin")
//         {
//             if (service.Update(id, auther))
//                 return NoContent();

//             return BadRequest();
//         }
//         else
//         {
//             var idToken = HttpContext.User.FindFirst("id")?.Value;
//             int.TryParse(idToken, out int typeId);
//             if (id == typeId)
//             {
//                 if (service.Update(id, auther))
//                     return NoContent();

//                 return BadRequest();
//             }
//             else
//             {
//                 return BadRequest("Unauthorized access");
//             }

//         }
//     }

//     [HttpDelete("{id}")]
//     [Authorize(policy: "Admin")]
//     public ActionResult Delete(int id)
//     {
//         if (service.Delete(id))
//             return Ok();

//         return NotFound();
//     }


// }
[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IService<Author> service;
    private readonly string autherRole;
    private ClaimsPrincipal claimsPrincipal;
    public AuthorController(IService<Author> service)
    {
        this.service = service;
        // כאן נשתמש בפונקציה כדי לאמת את הטוקן
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        claimsPrincipal = TokenService.ValidateToken(token);
        autherRole = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
    }

    [HttpGet]
    [Authorize(policy: "Author")]
    public ActionResult<IEnumerable<Author>> Get()
    {
        if (autherRole == "Admin")
        {
            return service.Get();
        }
        else if (autherRole == "Author")
        {
            var id = int.Parse(claimsPrincipal.FindFirst("Id")?.Value);
            return new List<Author> { service.Get(id) };
        }

        return BadRequest("Unauthorized access");
    }

    [HttpGet("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult<Author> Get(int id)
    {
        var auther = service.Get(id);
        if (auther == null)
            throw new ApplicationException("Author not found");
        return auther;
    }

    [HttpPost]
    [Authorize(policy: "Admin")]
    public ActionResult Post(Author newUser)
    {
        var newId = service.Insert(newUser);

        if (newId == -1)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    [Authorize(policy: "Author")]
    public ActionResult Put(int id, Author auther)
    {
        if (autherRole == "Admin")
        {
            if (service.Update(id, auther))
                return NoContent();

            return BadRequest();
        }
        else
        {
            var idToken = claimsPrincipal.FindFirst("Id")?.Value;
            int.TryParse(idToken, out int typeId);
            if (id == typeId)
            {
                if (service.Update(id, auther))
                    return NoContent();

                return BadRequest();
            }
            else
            {
                return BadRequest("Unauthorized access");
            }
        }
    }

    [HttpDelete("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult Delete(int id)
    {
        if (service.Delete(id))
            return Ok();

        return NotFound();
    }
}