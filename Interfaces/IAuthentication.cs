using project.Models;

namespace project.Interfaces;
public interface IAuthentication<T>  where T :IGeneric, IRole
{
   public List<T> Get();
}