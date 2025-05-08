using project.Models;

namespace project.Interfaces;

public interface ILogin<T> : IUser
{
    T GetCurrentUser();
    void SetCurrentUser(T user);

}
