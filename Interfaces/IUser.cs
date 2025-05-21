using project.Models;

namespace project.Interfaces;

public interface IUser : IRole, IGeneric, IAuthentication
{
    public string? Name { get; set; }

    public string? Address { get; set; }

    public DateOnly BirthDate { get; set; }

    string ToString();
}
