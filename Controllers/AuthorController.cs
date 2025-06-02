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
        try
        {
            var list = service.Get();
            if (list.Count <= 0)
                return BadRequest("Unauthorized access");


            return Ok(list);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Get: {ex.Message}");
            return StatusCode(500, "An error occurred while getting the authors.");
        }
    }

    [HttpGet("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult<Author> Get(int id)
    {
        try
        {
            System.Console.WriteLine("Get method called author: " + id);
            var author = service.Get(id);
            if (author == null)
            {
                Console.WriteLine("Author not found");
                return NotFound("Author not found");
            }
            System.Console.WriteLine("Author found " + author.ToString() + "1111");
            return Ok(author);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Get(id): {ex.Message}");
            return StatusCode(500, "An error occurred while getting the author.");
        }
    }

    [HttpPost]
    [Authorize(policy: "Admin")]
    public ActionResult Post(Author newUser)
    {
        try
        {
            var newId = service.Insert(newUser);
            if (newId == -1)
            {
                Console.WriteLine("Insert failed");
                return BadRequest("Failed to insert author.");
            }
            return CreatedAtAction(nameof(Post), new { Id = newId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Post: {ex.Message}");
            return StatusCode(500, "An error occurred while creating the author.");
        }
    }

    [HttpPut("{id}")]
    [Authorize(policy: "Author")]
    public ActionResult Put(int id, Author author)
    {
        try
        {
            if (service.Update(id, author))
                return NoContent();

            return BadRequest("Failed to update author.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Put: {ex.Message}");
            return StatusCode(500, "An error occurred while updating the author.");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult Delete(int id)
    {
        try
        {
            if (service.Delete(id))
                return Ok();
            return NotFound("Author not found for deletion.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Delete: {ex.Message}");
            return StatusCode(500, "An error occurred while deleting the author.");
        }
    }
}