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
    private readonly User user;
    
    public BookController(IService<Book> service, BookServiceJson bookService,User user)
    {
        System.Console.WriteLine(bookService + " bookService in BookController");
        this.bookService = bookService;
        this.user = user;
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
            
            System.Console.WriteLine("Author role in BookController");
            string name = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            return bookService.GetAuthorsBook(name) ?? new List<Book>();
        }
         return service.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        System.Console.WriteLine(id + " id in BookController");
        var book = service.Get(id);
        System.Console.WriteLine(book.Author + " book in BookController");
        var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        if (role == "Admin")
        {

            if (book == null)
                throw new ApplicationException("Book not found");
            return book;
        }
        else if (role == "Author")
        {
            System.Console.WriteLine("Auther role in getBYId in BookController");
            var authorName = HttpContext.User.FindFirst("Name")?.Value;
            System.Console.WriteLine(authorName.ToString() + " authorName in BookController");
            if (book == null)
                throw new ApplicationException("Book not ");
            if (book.Author != authorName.ToString())
                throw new ApplicationException(" unauthorized access");
            return book;
        }
        return BadRequest("Unauthorized access");
    }



    [HttpPost]
    public ActionResult Post(Book newBook)
    {
        System.Console.WriteLine("post in BookController");
        var authorName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

        // אם המשתמש הוא מנהל, אפשר ליצור ספר עבור כל סופר
        if (role == "Admin")
        {
            var newId = service.Insert(newBook);
            newBook.Id = newId;
            System.Console.WriteLine(newId + " newId in BookController");

            if (newId == -1)
            {
                return BadRequest("Failed to create new book.");
            }
            return CreatedAtAction(nameof(Post), new { Id = newId });
        }
        // אם המשתמש הוא Author, בדוק אם השם של הסופר תואם לשם של המשתמש
        else if (role == "Author")
        {
            if (newBook.Author != authorName)
            {
                return Forbid("Unauthorized: Author name does not match the logged-in user.");
            }

            var newId = service.Insert(newBook);
            newBook.Id = newId;
            System.Console.WriteLine(newId + " newId in BookController");

            if (newId == -1)
            {
                return BadRequest("Failed to create new book.");
            }
            return CreatedAtAction(nameof(Post), new { Id = newId });
        }

        return BadRequest("Unauthorized access.");
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Book book)
    {
        System.Console.WriteLine("put in BookController");
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