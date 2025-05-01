using project.Interfaces;

namespace project.Models;

public class Book : IGeneric
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public string? Author { get; set; }

    public double Price { get; set; }
    public DateOnly Date { get; set; }
}
