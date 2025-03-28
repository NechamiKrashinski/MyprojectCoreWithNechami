using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
namespace project.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : GenericController<Book>
{
    public BookController(IService<Book> service) : base(service)
    {
    }
}