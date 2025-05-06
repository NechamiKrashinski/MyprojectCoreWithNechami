using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
using project.Services;
using System.Collections.Generic;
using System.Security.Claims;

namespace project.Controllers;

[ApiController]
[Authorize(Policy = "Author")]
[Route("[controller]")]


public class BookController : ControllerBase
{
    private readonly IService<Book> service;
    private readonly BookServiceJson bookService;
   
    
    public BookController(IService<Book> service, BookServiceJson bookService)
    {
        System.Console.WriteLine(bookService + " bookService in BookController");
        this.bookService = bookService;
       
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
        System.Console.WriteLine("Get method in BookController called");
         return service.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        
       
        return BadRequest("Unauthorized access");
    }



    [HttpPost]
    public ActionResult Post(Book newBook)
    {
        
            var newId = service.Insert(newBook);
            System.Console.WriteLine(newId + " newId in post in BookController");
        
            if (newId == -1)
            {
                return BadRequest("Failed to create new book.");
            }
            if (newId == -2)
            {
                return Forbid("Unauthorized: Author name does not match the logged-in user.");
            }
            return CreatedAtAction(nameof(Post), new { Id = newId });
        
       }
        
           

            
    

       
    

    [HttpPut("{id}")]
    public ActionResult Put(int id, Book book)
    {
        var existingBook = service.Get(id);
        var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        var authorName = HttpContext.User.FindFirst("Name")?.Value;

        if (role == "Author")
        {
            if (existingBook == null)
                return NotFound("Book not found");

            // בדוק אם הספר שייך למשתמש
            if (existingBook.Author != authorName)
                return Forbid("Unauthorized access");

            // אם הכל בסדר, עדכן את הספר
            if (service.Update(id, book))
                return NoContent();
        }
        else if (role == "Admin")
        {
            System.Console.WriteLine("Admin role in put in BookController");
            // אם המשתמש הוא מנהל, אפשר לעדכן את הספר
            if (service.Update(id, book))
                return NoContent();
        }

        return BadRequest("Unauthorized access");
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existingBook = service.Get(id);
        var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        var authorName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

        if (role == "Author")
        {
            if (existingBook == null)
                return NotFound("Book not found");

            // בדוק אם הספר שייך למשתמש
            if (existingBook.Author != authorName)
                return Forbid("Unauthorized access");

            // אם הכל בסדר, מחק את הספר
            if (service.Delete(id))
                return Ok();
        }
        else if (role == "Admin")
        {
            // אם המשתמש הוא מנהל, אפשר למחוק את הספר
            if (service.Delete(id))
                return Ok();
        }

        return NotFound("Book not found or unauthorized access");
    }
    public class ClaimDto
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

}