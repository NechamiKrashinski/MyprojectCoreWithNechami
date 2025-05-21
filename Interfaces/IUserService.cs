namespace project.Interfaces;

public interface IUserService<T> where T : IUser
{
     //string Token { get; set; }
    List<T> Get();

    T Get(int id);

    int Insert(T newT);
    bool Update(int id, T t);
    bool Delete(int id);
}
