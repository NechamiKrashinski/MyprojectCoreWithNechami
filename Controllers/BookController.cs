using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;

namespace project.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(policy: "Author")]
public class BookController : ControllerBase
{
    private readonly IService<Book> service;
    private readonly IService<Author> service2;

    private string? token;

    public BookController(IService<Book> service, IService<Author> service2)
    {
        this.service = service;
        this.service2 = service2;
    }

    private void SetToken()
    {
        token = HttpContext.Request.Cookies["AuthToken"]!;
        if (!string.IsNullOrEmpty(token))
        {
          //  service.Token = token;
           // service2.Token = token;
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        var list = service.Get();
        if (list.Count <= 0)
        {
            return BadRequest("Unauthorized access");
        }
        return Ok(list);
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {

        Console.WriteLine($"Get({id}) called");
        var book = service.Get(id);
        if (book == null)
        {
            Console.WriteLine("Book not found");
            throw new ApplicationException("Book not found");
        }
        Console.WriteLine($"Returning book with Id: {id}");
        return book;
    }

    [HttpPost]
    public ActionResult Post(Book newBook)
    {

        Console.WriteLine("Post() called---------------");
        var newId = service.Insert(newBook);
        if (newId == -1)
        {
            Console.WriteLine("Insert failed");
            return BadRequest();
        }
        Console.WriteLine($"Book created with Id: {newId}");
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Book book)
    {

        Console.WriteLine($"Put({id}) called");
        if (service.Update(id, book))
        {
            Console.WriteLine("Book updated successfully");
            return NoContent();
        }
        Console.WriteLine("Update failed");
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {

        Console.WriteLine($"Delete({id}) called");
        if (service.Delete(id))
        {
            Console.WriteLine("Book deleted successfully");
            return Ok();
        }

        Console.WriteLine("Book not found for deletion");
        return NotFound();
    }
}
