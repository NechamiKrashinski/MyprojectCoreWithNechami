namespace project.Interfaces;

public interface ILogin<T> where T : IGeneric, IRole
{
   public  string Login(int id);
}

