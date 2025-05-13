using project.Interfaces;
using project.Models;

namespace project.Services;

public class AuthorServiceJson : GetFuncService<Author>, IService<Author>
{
    private readonly int authorId;
    private readonly Role role;

    public AuthorServiceJson(IHostEnvironment env)
        : base(env)
    {
        authorId = CurrentUser.Id;
        role = CurrentUser.role;
    }

    internal int Id(string name)
    {
        return MyList.FirstOrDefault(b => b.Name == name).Id;
    }

    public override List<Author> Get()
    {
        Console.WriteLine("Get method called " + role.ToString() + " " + authorId.ToString());
        if (role == Role.Author)
        {
            return new List<Author> { Get(authorId) }
                .Where(a => a != null)
                .ToList();
        }
        else if (role == Role.Admin)
        {
            return MyList;
        }
        return new List<Author>();
    }

    public Author Get(int id)
    {
        var author = MyList.FirstOrDefault(b => b.Id == id);
        if (author == null)
        {
            return null;
        }
        return author;
    }

    public int Insert(Author newUser)
    {
        if (
            newUser == null
            || string.IsNullOrWhiteSpace(newUser.Name)
            || string.IsNullOrWhiteSpace(newUser.Address)
        )
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

    public bool Update(int id, Author author)
    {
        if (role == Role.Admin || role == Role.Author && authorId == id)
        {
            if (author == null)
            {
                return false;
            }

            if (author.Id != id)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(author.Name))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(author.Address))
            {
                return false;
            }

            // if (author.BirthDate.ToDateTime(TimeOnly.MinValue).Date <= DateTime.Today)
            // {
            //     Console.WriteLine("Author birth date is not valid.");
            //     return false;
            // }

            Console.WriteLine("Validation succeeded.");
            {
                if (
                    author == null
                    || author.Id != id
                    || string.IsNullOrWhiteSpace(author.Name)
                    || string.IsNullOrWhiteSpace(author.Address)
                )
                {
                    return false;
                }

                var currentUser = MyList.FirstOrDefault(u => u.Id == id);
                if (currentUser == null)
                {
                    return false;
                }

                currentUser.Name = author.Name;
                currentUser.Address = author.Address;
                currentUser.BirthDate = author.BirthDate;
                saveToFile();
                Console.WriteLine("Update successful.");
                return true;
            }
        }
        return false;
    }

    public bool Delete(int id)
    {
        var currentT = MyList.FirstOrDefault(b => b.Id == id);
        if (currentT == null)
            return false;

        MyList.Remove(currentT);
        saveToFile();
        return true;
    }

    // protected void saveToFile()
    // {
    //     File.WriteAllText("../data/author.json", JsonSerializer.Serialize(MyList));
    // }
}
