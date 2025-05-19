namespace project.Interfaces;

public interface ILogin<T> where T : IGeneric, IUser 
{
   public  string Login(string password,string name);
    
}

