using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IService<User> service;

    public UserController(IService<User> service)
    {
        this.service = service;
    }

    [HttpGet]
    [Authorize(policy: "User")]
    public ActionResult<IEnumerable<User>> Get()
    {
         var userType = HttpContext.User.FindFirst("type")?.Value;
            if(userType == "Admin")
            {
                return service.Get();
            }
            else if(userType == "User")
            {
                var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);
            }

        return service.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var user = service.Get(id);
        if (user == null)
            throw new ApplicationException("User not found");
        return user;
    }

    [HttpPost]
    public ActionResult Post(User newUser)
    {
        var newId = service.Insert(newUser);

        if (newId == -1)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, User user)
    {
        if (service.Update(id, user))
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
