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

    public BookController(IService<Book> service)
    {
        this.service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
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
