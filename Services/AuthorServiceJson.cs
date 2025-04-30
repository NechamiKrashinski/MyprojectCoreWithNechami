using project.Models;

namespace project.Services;

public class UserServiceJson : ServiceJson<Author>
{
    public UserServiceJson(IHostEnvironment env)
        : base(env) { }

    public override int Insert(Author newUser)
    {
        if (newUser == null)
        {
            return -1;
        }

        if (string.IsNullOrWhiteSpace(newUser.Name))
        {
            return -1;
        }

        if (string.IsNullOrWhiteSpace(newUser.Address))
        {
            return -1;
        }

        if (newUser.BirthDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
        {
            return -1;
        }

        int maxId = MyList.Any() ? MyList.Max(u => u.Id) : 0;

        newUser.Id = maxId + 1;
        MyList.Add(newUser);
        saveToFile();

        return newUser.Id;
    }

    public override bool Update(int id, Author author)
    {
        if (
            author == null
            || author.Id != id
            || string.IsNullOrWhiteSpace(author.Name)
            || string.IsNullOrWhiteSpace(author.Address)
            || author.BirthDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now
        )
            return false;

        var currentUser = MyList.FirstOrDefault(u => u.Id == id);
        if (currentUser == null)
            return false;

        currentUser.Name = author.Name;
        currentUser.Address = author.Address;
        currentUser.BirthDate = author.BirthDate;
        saveToFile();
        return true;
    }
}
