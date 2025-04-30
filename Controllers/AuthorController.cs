using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
using project.Services;

namespace project.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IService<Author> service;
    private readonly string authorRole;

    public AuthorController(IService<Author> service)
    {
        this.service = service;
        authorRole = HttpContext.User.FindFirst("Role")?.Value;
    }

    [HttpGet]
    [Authorize(policy: "Author")]
    public ActionResult<IEnumerable<Author>> Get()
    {
        if (authorRole == "Admin")
        {
            return service.Get();
        }
        else if (authorRole == "Author")
        {
            var idClaim = HttpContext.User.FindFirst("Id")?.Value;
            if (idClaim == null)
            {
                throw new ApplicationException("User ID not found");
            }
            var id = int.Parse(idClaim);
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
        var newId = service.Insert(newUser);

        if (newId == -1)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    [Authorize(policy: "Author")]
    public ActionResult Put(int id, Author author)
    {
        if (authorRole == "Admin")
        {
            if (service.Update(id, author))
                return NoContent();

            return BadRequest();
        }
        else
        {
            var idToken = HttpContext.User.FindFirst("Id")?.Value;
            int.TryParse(idToken, out int typeId);
            if (id == typeId)
            {
                if (service.Update(id, author))
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
