
namespace project.Interfaces;

public interface IAuthentication<T>
    where T : IGeneric, IRole
{
    List<T> Get();
}
