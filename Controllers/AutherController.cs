using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
using System.Collections.Generic;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IService<Auther> service;
    private readonly string autherType;

    public UserController(IService<Auther> service)
    {
        this.service = service;
        autherType = HttpContext.User.FindFirst("type")?.Value;
    }

    [HttpGet]
    [Authorize(policy: "Auther")]
    public ActionResult<IEnumerable<Auther>> Get()
    {

        if (autherType == "Admin")
        {
            return service.Get();
        }
        else if (autherType == "Auther")
        {
            var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);
            return new List<Auther> { service.Get(id) };
        }

        return BadRequest("Unauthorized access");
    }

    [HttpGet("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult<Auther> Get(int id)
    {
        var auther = service.Get(id);
        if (auther == null)
            throw new ApplicationException("Auther not found");
        return auther;
    }

    [HttpPost]
    [Authorize(policy: "Admin")]
    public ActionResult Post(Auther newUser)
    {
        var newId = service.Insert(newUser);

        if (newId == -1)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    [Authorize(policy: "Auther")]
    public ActionResult Put(int id, Auther auther)
    {
        if (autherType == "Admin")
        {
            if (service.Update(id, auther))
                return NoContent();

            return BadRequest();
        }
        else
        {
            var idToken = HttpContext.User.FindFirst("id")?.Value;
            int.TryParse(idToken, out int typeId);
            if (id == typeId)
            {
                if (service.Update(id, auther))
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
