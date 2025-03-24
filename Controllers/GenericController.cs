using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
namespace project.Controllers;


[ApiController]
[Route("[controller]")]
public class GenericController<T> : ControllerBase where T : IGeneric
{
    private readonly IService<T> service;

    public GenericController(IService<T> service)
    {
        this.service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<T>> Get()
    {
        return service.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<T> Get(int id)
    {
        var item = service.Get(id);
        if (item == null)
            throw new ApplicationException($"{typeof(T).Name} not found");
        return item;
    }

    [HttpPost]
    public ActionResult Post(T newItem)
    {
        System.Console.WriteLine("------------////***************************");
        var newId = service.Insert(newItem);
        System.Console.WriteLine("------------////***************************");

        if (newId == -1)
        {
            System.Console.WriteLine("------------////***************************");

            return BadRequest();
        }
        System.Console.WriteLine("------------////***************************");

        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, T item)
    {
        if (service.Update(id, item))
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
