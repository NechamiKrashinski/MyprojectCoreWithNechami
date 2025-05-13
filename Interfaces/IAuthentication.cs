namespace project.Interfaces;

public interface IAuthentication<T>
    where T : IUser
{
    List<T> Get();
}
