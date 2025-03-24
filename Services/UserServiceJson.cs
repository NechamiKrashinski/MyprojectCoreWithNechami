using project.Models;

namespace project.Services;

public class UserServiceJson : ServiceJson<User>
{

    public UserServiceJson(IHostEnvironment env) : base(env, "user.json")
    {

    }
    public override int Insert(User newUser)
    {
        System.Console.WriteLine("inset 11111" + newUser);
        if (newUser == null)
        {
            Console.WriteLine("Error: newUser is null.");
            return -1;
        }

        if (string.IsNullOrWhiteSpace(newUser.Name))
        {
            Console.WriteLine("Error: Name is required.");
            return -1;
        }

        if (string.IsNullOrWhiteSpace(newUser.Address))
        {
            Console.WriteLine("Error: Address is required.");
            return -1;
        }

        if (newUser.BirthDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
        {
            Console.WriteLine("Error: BirthDate must be in the past.");
            return -1;
        }
        System.Console.WriteLine("inset 222222");

        int maxId = MyList.Any() ? MyList.Max(u => u.Id) : 0;
        System.Console.WriteLine("inset 3333");

        newUser.Id = maxId + 1;
        MyList.Add(newUser);
        saveToFile();
        System.Console.WriteLine("inset 4444");

        return newUser.Id;
    }



    public override bool Update(int id, User user)
    {

        if (user == null || user.Id != id ||
               string.IsNullOrWhiteSpace(user.Name) ||
               string.IsNullOrWhiteSpace(user.Address) ||
              user.BirthDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now)
            return false;

        var currentUser = MyList.FirstOrDefault(u => u.Id == id);
        if (currentUser == null)
            return false;

        currentUser.Name = user.Name;
        currentUser.Address = user.Address;
        currentUser.BirthDate = user.BirthDate;
        saveToFile();
        return true;

    }



}