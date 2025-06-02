namespace project.Interfaces;

public interface IUserService<T>:IService<T>
    where T : IUser
{
    bool isAuth { get; set; }
}
