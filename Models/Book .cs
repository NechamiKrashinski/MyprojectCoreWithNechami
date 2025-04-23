namespace project.Models;


public class Book:IGeneric
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string?  Auther{get; set;}

    public double Price { get; set; }
    public DateOnly Date { get; set; }


}
