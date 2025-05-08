namespace project.Interfaces;

public interface IAuthentication<T>
where T :ILogin<T>{
    List<T> Get();
}
