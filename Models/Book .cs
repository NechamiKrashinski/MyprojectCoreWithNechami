using project.Interfaces;

namespace project.Models;

public class Book : IGeneric
{
    public int Id { get; set; }

    public string? Name { get; set; }

<<<<<<< HEAD
    public string?  Author{get; set;}
=======
    public string? Author { get; set; }
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa

    public double Price { get; set; }
    public DateOnly Date { get; set; }
}
