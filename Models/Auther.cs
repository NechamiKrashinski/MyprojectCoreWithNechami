namespace project.Models;

public class Auther : IGeneric
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public DateOnly BirthDate { get; set; }

    public Role Role { get; set; } = Role.Auther;

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Address: {Address}, BirthDate: {BirthDate}";
    }
}
