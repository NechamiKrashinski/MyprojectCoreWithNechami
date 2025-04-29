using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
using project.Services;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IService<Auther> service;
    private readonly AuthenticationService authenticationService;
    private readonly string autherRole;

    public UserController(IService<Auther> service)
    {
        this.service = service;
        autherRole = HttpContext.User.FindFirst("role")?.Value;
    }

    [HttpGet]
    [Authorize(policy: "Auther")]
    public ActionResult<IEnumerable<Auther>> Get()
    {
        if (autherRole == "Admin")
        {
            return service.Get();
        }
        else if (autherRole == "Auther")
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
        if (autherRole == "Admin")
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
    [Authorize(policy: "Admin")]
    public ActionResult Delete(int id)
    {
        if (service.Delete(id))
            return Ok();

        return NotFound();
    }

    public ActionResult<Auther> Login([FromBody] Auther auther)
    {
        var authenticate = authenticationService.Get().FirstOrDefault(a => a.Id == auther.Id);
        if (auther == null)
            throw new ApplicationException("Auther not found");
        return auther;
    }
}
