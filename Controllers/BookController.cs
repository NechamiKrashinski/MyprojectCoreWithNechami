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
    private readonly IItemService<Book> service;

    public BookController(IItemService<Book> service)
    {
        this.service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        try
        {
            System.Console.WriteLine("Get method called2 book+++++++++++++++++++++ ");
            var list = service.Get();
            if (list.Count <= 0)
            {
                return BadRequest("Unauthorized access");
            }
            System.Console.WriteLine(
                "Get method called2 book-******-------------------*********** 2"
            );
            foreach (var item in list)
            {
                System.Console.WriteLine(item.ToString());
            }
            System.Console.WriteLine(
                "Get method called2 book3***********************************2"
            );

            return Ok(list);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Get: {ex.Message}");
            return StatusCode(500, "An error occurred while getting the books.");
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        try
        {
            Console.WriteLine($"Get({id}) called");
            var book = service.Get(id);
            if (book == null)
            {
                Console.WriteLine("Book not found");
                return NotFound("Book not found");
            }
            Console.WriteLine($"Returning book with Id: {id}");
            return book;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Get(id): {ex.Message}");
            return StatusCode(500, "An error occurred while getting the book.");
        }
    }

    [HttpPost]
    public ActionResult Post(Book newBook)
    {
        try
        {
            Console.WriteLine("Post() called-------------1111--" + newBook.ToString());
            var newId = service.Insert(newBook);
            if (newId == -1)
            {
                Console.WriteLine("Insert failed");
                return BadRequest("Failed to insert book.");
            }
            Console.WriteLine($"Book created with Id: {newId}");
            return CreatedAtAction(nameof(Post), new { Id = newId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Post: {ex.Message}");
            return StatusCode(500, "An error occurred while creating the book.");
        }
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Book book)
    {
        try
        {
            Console.WriteLine($"Put({id}) called");
            if (service.Update(id, book))
            {
                Console.WriteLine("Book updated successfully");
                return NoContent();
            }
            Console.WriteLine("Update failed");
            return BadRequest("Failed to update book.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Put: {ex.Message}");
            return StatusCode(500, "An error occurred while updating the book.");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            Console.WriteLine($"Delete({id}) called");
            if (service.Delete(id))
            {
                Console.WriteLine("Book deleted successfully");
                return Ok();
            }

            Console.WriteLine("Book not found for deletion");
            return NotFound("Book not found for deletion.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Delete: {ex.Message}");
            return StatusCode(500, "An error occurred while deleting the book.");
        }
    }
}
