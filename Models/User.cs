
using project.Interfaces;

namespace project.Models;


public class User:IGeneric
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string?  Address{get; set;}

    public DateOnly BirthDate { get; set; }


}