using project.Interfaces;
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
        Console.WriteLine($"Update called with id: {id}");
        Console.WriteLine($"Author provided: {author}");

        if (author == null)
        {
            Console.WriteLine("Author is null.");
            return false;
        }

        if (author.Id != id)
        {
            Console.WriteLine("Author ID does not match.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(author.Name))
        {
            Console.WriteLine("Author name is empty or whitespace.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(author.Address))
        {
            Console.WriteLine("Author address is empty or whitespace.");
            return false;
        }

        // if (author.BirthDate.ToDateTime(TimeOnly.MinValue).Date <= DateTime.Today)
        // {
        //     Console.WriteLine("Author birth date is not valid.");
        //     return false;
        // }
       

        Console.WriteLine("Validation succeeded.");

        var currentUser = MyList.FirstOrDefault(u => u.Id == id);
        if (currentUser == null)
        {
            Console.WriteLine("Current user not found.");
            return false;
        }

        Console.WriteLine($"Updating user: {currentUser.Name}");
        currentUser.Name = author.Name;
        currentUser.Address = author.Address;
        currentUser.BirthDate = author.BirthDate;
        saveToFile();
        Console.WriteLine("Update successful.");
        return true;
    }
}