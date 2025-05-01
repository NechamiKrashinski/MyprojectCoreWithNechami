namespace project.Interfaces;

public interface ILogin<T>
    where T : IUser
{
    public string Login(int id);
}
