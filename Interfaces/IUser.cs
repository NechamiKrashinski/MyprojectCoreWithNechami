using project.Models;

namespace project.Interfaces;

public interface IUser : IRole, IGeneric
{
   public string email { get; set; }
   public string password { get; set; }

}
