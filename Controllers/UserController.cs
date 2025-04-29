using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
using System.Collections.Generic;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IService<User> service;
    private readonly string userType;

    public UserController(IService<User> service)
    {
        this.service = service;
        userType = HttpContext.User.FindFirst("type")?.Value;
    }

    [HttpGet]
    [Authorize(policy: "User")]
    public ActionResult<IEnumerable<User>> Get()
    {

        if (userType == "Admin")
        {
            return service.Get();
        }
        else if (userType == "User")
        {
            var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);
            return new List<User> { service.Get(id) };
        }

        return BadRequest("Unauthorized access");
    }

    [HttpGet("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult<User> Get(int id)
    {
        var user = service.Get(id);
        if (user == null)
            throw new ApplicationException("User not found");
        return user;
    }

    [HttpPost]
    [Authorize(policy: "Admin")]
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
    [Authorize(policy: "User")]
    public ActionResult Put(int id, User user)
    {
        if (userType == "Admin")
        {
            if (service.Update(id, user))
                return NoContent();

            return BadRequest();
        }
        else
        {
            var idToken = HttpContext.User.FindFirst("id")?.Value;
            int.TryParse(idToken, out int typeId);
            if (id == typeId)
            {
                if (service.Update(id, user))
                    return NoContent();

                return BadRequest();
            }
            else
            {
                return BadRequest("Unauthorized access");
            }

        }
    }

    [HttpDelete("{id}")]
    [Authorize(policy:"Admin")]
    public ActionResult Delete(int id)
    {
        if (service.Delete(id))
            return Ok();

        return NotFound();
    }
}
