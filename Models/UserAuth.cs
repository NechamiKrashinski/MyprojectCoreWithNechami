using project.Interfaces;

namespace project.Models;

public class UserAuth : IUser
{
    public int Id { get; set; }
    public Role role { get; set; }
}
