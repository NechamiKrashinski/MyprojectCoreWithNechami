using project.Interfaces;
using project.Models;

public class Author : IUser
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public DateOnly BirthDate { get; set; }

    public Role role { get; set; } = Role.Reader;

    public required string emailValue; // שונה לשם אחר
    public string email
    {
        get => emailValue;
        set
        {
            if (!value.Contains("@"))
            {
                throw new ArgumentException("Email must contain '@'.");
            }
            emailValue = value;
        }
    }

    public required string passwordValue; // שונה לשם אחר
    public string password
    {
        get => passwordValue;
        set
        {
            if (value.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long.");
            }
            passwordValue = value;
        }
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Address: {Address}, BirthDate: {BirthDate}";
    }
}

