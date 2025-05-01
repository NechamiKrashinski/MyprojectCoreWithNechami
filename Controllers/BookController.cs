using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;

using System.Collections.Generic;
using System.Security.Claims;

namespace project.Controllers;

[ApiController]
[Authorize(Policy = "Admin")]
[Route("[controller]")]


public class BookController : ControllerBase
{
    private readonly IService<Book> service;

    public BookController(IService<Book> service)
    {
        this.service = service;
    }
// [HttpGet]
// public ActionResult<IEnumerable<ClaimDto>> GetClaims()
// {
//     if (!HttpContext.User.Identity.IsAuthenticated)
//     {
//         return Unauthorized();
//     }

//     var claims = HttpContext.User.Claims.Select(c => new ClaimDto
//     {
//         Type = c.Type,
//         Value = c.Value
//     }).ToList();

//     return Ok(claims);
// }
    [HttpGet]

    public ActionResult<IEnumerable<Book>> Get()
    {
        var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        if (role == "Admin")
        {
            System.Console.WriteLine("Admin role in BookController");
            return service.Get();
        }
        else if (role == "Author")
        {
             System.Console.WriteLine("Auther role in BookController");
            var id = int.Parse(HttpContext.User.FindFirst("Id")?.Value);
            return new List<Book> { service.Get(id) };
        }
        return service.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        var book = service.Get(id);
        if (book == null)
            throw new ApplicationException("Book not found");
        return book;
    }

    [HttpPost]
    public ActionResult Post(Book newBook)
    {
        var newId = service.Insert(newBook);
        if (newId == -1)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Book book)
    {
        if (service.Update(id, book))
            return NoContent();
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (service.Delete(id))
            return Ok();

        return NotFound();
    }

   
}

public class ClaimDto
{
    public string Type { get; set; }
    public string Value { get; set; }
}