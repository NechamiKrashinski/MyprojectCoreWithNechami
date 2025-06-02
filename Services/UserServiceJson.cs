using project.Interfaces;
using project.Models;

namespace project.Services;

public class UserServiceJson<T, I> : ReadJson<T>, IUserService<T>
    where T : IUser
    where I : IItem
{
    private readonly int authorId;
    private readonly Role role;
    public bool isAuth { get; set; } = false;

     private readonly Lazy<IItemService<Book>> itemService;

    public UserServiceJson(IHostEnvironment env, Lazy<IItemService<Book>> itemService)
        : base(env)
    {
        this.itemService = itemService;
        authorId = CurrentUser.Id;
        role = CurrentUser.role;
    }
    public override List<T> Get()
    {
        System.Console.WriteLine("Role!@#$%^&*()    "+role.ToString());
        if (role == Role.Author)
        {
            var authorList = new List<T> { Get(authorId) };

            var filteredUsers = authorList.Where(a => a != null);

            var result = filteredUsers.ToList();

            return result;
        }
        else if (role == Role.Admin)
        {
            return MyList;
        }
        else if (isAuth)
        {
            isAuth = false;
            return MyList;
        }
        isAuth = false;
        throw new Exception("Unauthorized access");
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
            throw new Exception("Insert failed: User is null or invalid");
        }

        if (newUser.BirthDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
        {
            throw new Exception("Insert failed: User birth date is not valid");
        }

        int maxId = MyList.Any() ? MyList.Max(u => u.Id) : 0;
        newUser.Id = maxId + 1;
        MyList.Add(newUser);
        saveToFile();

        return newUser.Id;
    }

    public bool Update(int id, T author)
    {
        if (
            !(role == Role.Admin || (role == Role.Author && authorId == id))
            || author == null
            || author.Id != id
            || string.IsNullOrWhiteSpace(author.Name)
            || string.IsNullOrWhiteSpace(author.Address)
        )
        {
            throw new Exception("Update failed: Invalid user data");
        }

        var currentUser = MyList.FirstOrDefault(u => u.Id == id);
        if (currentUser == null)
        {
            throw new Exception("Update failed: User not found");
        }

        currentUser.Name = author.Name;
        currentUser.Address = author.Address;
        currentUser.BirthDate = author.BirthDate;
        saveToFile();
        Console.WriteLine("Update successful.");
        return true;
    }

    // public bool Delete(int id)
    // {
    //     var currentT = MyList.FirstOrDefault(b => b.Id == id);
    //     if (currentT == null)
    //         throw new Exception("Delete failed: Book not found");
    //     // bookService.Get()
    //     //     .Where(b => b.AuthorId == id)
    //     //     .ToList()
    //     //     .ForEach(b => bookService.Delete(b.Id));
    //     MyList.Remove(currentT);
    //     saveToFile();
    //     return true;
    // }

    public bool Delete(int id)
    {
        var currentUser = MyList.FirstOrDefault(b => b.Id == id);
        if (currentUser == null)
            throw new Exception("Delete failed: User not found");

        // מחיקת כל הפריטים של המשתמש
        var userItems = itemService.Value.Get().Where(i => i.UserId == id).ToList();
        foreach (var item in userItems)
        {
            itemService.Value.Delete(item.Id);
        }

        MyList.Remove(currentUser);
        saveToFile();
        return true;
    }
}
