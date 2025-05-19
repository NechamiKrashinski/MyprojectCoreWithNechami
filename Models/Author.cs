using project.Interfaces;

namespace project.Models;


public class Author:IGeneric,IUser

{
    public int Id { get; set; }

    public string Name { get; set; }

  public string Password { get; set; } = string.Empty;
    public Role role { get; set; } = Role.Author;
    public string?  Address{get; set;}

    public DateOnly BirthDate { get; set; }


      public override string ToString()

    {
        return $"Id: {Id}, Name: {Name}, Address: {Address}, BirthDate: {BirthDate}";
    }
}
