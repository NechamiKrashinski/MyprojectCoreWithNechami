using project.Interfaces;

namespace project.Models;

public class Book : IItem
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public string? Author { get; set; }

    // public double Price { get; set; }
    public DateOnly Date { get; set; }
    public int UserId { get; set; }
    DateTime IItem.Date { get ; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Author: {Author}, AuthorId: {UserId}, Date: {Date}";
    }
}
