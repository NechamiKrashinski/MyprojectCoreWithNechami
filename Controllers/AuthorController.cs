using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;

namespace project.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IUserService<Author> service;

    public AuthorController(IUserService<Author> service)
    {
        this.service = service;
    }

    [HttpGet]
    [Authorize(policy: "Author")]
    public ActionResult<IEnumerable<Author>> Get()
    {
        System.Console.WriteLine("Get method called1----------------------");
        var list = service.Get();
        System.Console.WriteLine("Get method called " + list[0].ToString());

        if (list.Count <= 0)
            return BadRequest("Unauthorized access");
        System.Console.WriteLine("Get method called " + list[0].ToString());
        System.Console.WriteLine("Get method called2-******************************");

        return Ok(list);
    }

    [HttpGet("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult<Author> Get(int id)
    {
        try
        {
            System.Console.WriteLine("Get method called " + id.ToString());
            var author = service.Get(id);
            if (author == null)
                throw new ApplicationException("Author not found");
            System.Console.WriteLine("Author  found" + author.ToString() + "1111");
            return author;
        }
        catch
        {
            throw new ApplicationException("Author not found");
        }
    }

    [HttpPost]
    [Authorize(policy: "Admin")]
    public ActionResult Post(Author newUser)
    {
        var newId = service.Insert(newUser);
        if (newId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    [Authorize(policy: "Author")]
    public ActionResult Put(int id, Author author)
    {
        if (service.Update(id, author))
            return NoContent();

        return BadRequest();
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
