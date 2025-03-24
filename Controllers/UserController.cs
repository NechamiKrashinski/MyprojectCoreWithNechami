using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
namespace project.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : GenericController<User>
{
    public UserController(IService<User> service) : base(service)
    {
    }
}