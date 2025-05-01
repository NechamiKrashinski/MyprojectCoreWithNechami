using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;

namespace project.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IService<Author> service;
    private string? token;
    public AuthorController(IService<Author> service)
    {
        this.service = service;
    }

    private void SetToken()
    {
        token = HttpContext.Request.Cookies["AuthToken"]!;
        if (!string.IsNullOrEmpty(token))
            service.Token = token;
    }


    [HttpGet]
    [Authorize(policy: "Author")]
    public ActionResult<IEnumerable<Author>> Get()
    {
        SetToken();
        var list = service.Get();
        if (list.Count <= 0)
            return BadRequest("Unauthorized access");
        return list;
    }

    [HttpGet("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult<Author> Get(int id)
    {
        SetToken();
        var author = service.Get(id);
        if (author == null)
            throw new ApplicationException("Author not found");
        return author;
    }

    [HttpPost]
    [Authorize(policy: "Admin")]
    public ActionResult Post(Author newUser)
    {
        SetToken();
        var newId = service.Insert(newUser);
        if (newId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    [Authorize(policy: "Author")]
    public ActionResult Put(int id, Author author)
    {
        SetToken();
        if (service.Update(id, author))
            return NoContent();

        return BadRequest();
    }

    [HttpDelete("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult Delete(int id)
    {
        SetToken();
        if (service.Delete(id))
            return Ok();
        return NotFound();
    }
}
