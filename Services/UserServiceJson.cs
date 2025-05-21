using project.Interfaces;
using project.Models;

namespace project.Services;

public class UserServiceJson<T> : GetFuncService<T>, IUserService<T>
    where T : IUser
{
    private readonly int authorId;
    private readonly Role role;
    private readonly IService<Book> bookService;

    public UserServiceJson(IHostEnvironment env)
        : base(env)
    {
        authorId = CurrentUser.Id;
        role = CurrentUser.role;
    }

    public override List<T> Get()
    {
        Console.WriteLine("Get method called " + role.ToString() + " " + authorId.ToString());
        if (role == Role.Author)
        {
            System.Console.WriteLine("Author role");
            var authorList = new List<T> { Get(authorId) };
            Console.WriteLine("Author list created.");

            var filteredUsers = authorList.Where(a => a != null);
            Console.WriteLine($"Number of authors after filtering: {filteredUsers.Count()}");

            var result = filteredUsers.ToList();
            Console.WriteLine("Result converted to list." + result[0].ToString());

            return result;
        }
        else if (role == Role.Admin)
        {
            return MyList;
        }
        return new List<T>();
    }

    public T Get(int id)
    {
        var author = MyList.FirstOrDefault(b => b.Id == id);
        if (author == null)
        {
            throw new Exception("Author not found");
        }
        return author;
    }

    public int Insert(T newUser)
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

    public bool Update(int id, T author)
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
        // bookService.Get()
        //     .Where(b => b.AuthorId == id)
        //     .ToList()
        //     .ForEach(b => bookService.Delete(b.Id));
        MyList.Remove(currentT);
        saveToFile();
        return true;
    }
}
