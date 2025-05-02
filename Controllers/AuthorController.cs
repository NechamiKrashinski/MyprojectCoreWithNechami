using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;


using project.Services;
using System.Collections.Generic;
using System.Security.Claims;
namespace project.Controllers;



[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IService<Author> service;

   
    private ClaimsPrincipal claimsPrincipal;
    public AuthorController(IService<Author> service)
    {
        this.service = service;
        System.Console.WriteLine("AuthorController constructor called");
        
    }

    [HttpGet]
    [Authorize(policy: "Author")]
    public ActionResult<IEnumerable<Author>> Get()
    {var  authorRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        System.Console.WriteLine(authorRole + " authorRole in AuthorController get");
        if (authorRole == "Admin")
        {
            return service.Get();
        }
        else if (authorRole == "Author")
        {
            var id = int.Parse(HttpContext.User.FindFirst("Id")?.Value);
            return new List<Author> { service.Get(id) };

        }

        return BadRequest("Unauthorized access");
    }

    [HttpGet("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult<Author> Get(int id)
    {

        var author = service.Get(id);
        if (author == null)
            throw new ApplicationException("Author not found");
        return author;

    }

    [HttpPost]
    [Authorize(policy: "Admin")]
    public ActionResult Post(Author newUser)
    {
        System.Console.WriteLine("AuthorController post method called");
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
        System.Console.WriteLine("AuthorController put method called");
        var  authorRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
       System.Console.WriteLine(authorRole + " authorRole in AuthorController put");
        if (authorRole == "Admin")
        {
            if (service.Update(id, auther))

                return NoContent();

            return BadRequest();
        }
        else
        {

            var idToken = HttpContext.User.FindFirst("Id")?.Value;
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

