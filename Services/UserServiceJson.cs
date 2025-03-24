using project.Models;

namespace project.Services;

public class UserServiceJson : ServiceJson<User>
{

    public UserServiceJson(IHostEnvironment env) : base(env, "user.json")
    {

    }
    public override int Insert(User newUser)
    {
        if (newUser == null || string.IsNullOrWhiteSpace(newUser.Name) ||
               string.IsNullOrWhiteSpace(newUser.Address) || newUser.BirthDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now)
            return -1;

        int maxId = MyList.Any() ? MyList.Max(u => u.Id) : 0; // Ensure there are users
        newUser.Id = maxId + 1;
        MyList.Add(newUser);
        saveToFile();
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