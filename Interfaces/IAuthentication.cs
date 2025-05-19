
using project.Models;

namespace project.Interfaces;
public interface IAuthentication<T>  where T :IGeneric 
{
   public List<T> Get();
}

