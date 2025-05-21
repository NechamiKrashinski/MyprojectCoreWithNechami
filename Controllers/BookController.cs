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


    private string? token;

    public BookController(IService<Book> service)
    {
        this.service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        System.Console.WriteLine("Get method called2 book+++++++++++++++++++++ ");
        var list = service.Get();
        if (list.Count <= 0)
        {
            return BadRequest("Unauthorized access");
        }
        System.Console.WriteLine("Get method called2 book-******-------------------*********** 2");
        foreach (var item in list)
        {
            System.Console.WriteLine(item.ToString());
        }
        System.Console.WriteLine("Get method called2 book3***********************************2");

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
        Console.WriteLine("Post() called-------------1111--" + newBook.ToString());
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
