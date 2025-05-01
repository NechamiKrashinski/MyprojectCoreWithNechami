using project.Interfaces;
<<<<<<< HEAD
namespace project.Models;


public class Author:IGeneric,IRole
=======

namespace project.Models;

public class Author : IGeneric, IRole
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
{
    public int Id { get; set; }

    public string? Name { get; set; }

<<<<<<< HEAD
    public Role role { get; set; } = Role.Author;
    public string?  Address{get; set;}

    public DateOnly BirthDate { get; set; }


      public override string ToString()
=======
    public string? Address { get; set; }

    public DateOnly BirthDate { get; set; }

    public Role role { get; set; } = Role.Author;

    public override string ToString()
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
    {
        return $"Id: {Id}, Name: {Name}, Address: {Address}, BirthDate: {BirthDate}";
    }
}
