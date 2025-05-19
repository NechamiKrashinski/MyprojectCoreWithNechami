
using project.Interfaces;
using project.Models;

namespace project.Services;

public class AuthorServiceJson : ServiceJson<Author>
{

private readonly CurrentUserService user;

private readonly IService<Book> bookService;

    public AuthorServiceJson(IHostEnvironment env ,IService<Book> bookService, CurrentUserService currentUserService) : base(env)
    {
        user = currentUserService;
        this.bookService = bookService;
       
        
    }

   public override List<Author> Get()
    {
        var authorRole = user.Role;

        if (authorRole == "Admin")
        {
            return MyList;
        }
        else if (authorRole == "Author")
        {
            var id = user.Id;
            return new List<Author> { Get(id) };
        }

        return null; // Unauthorized
    }

    public override Author Get(int id)
    {
        var authorRole = user.Role;

        if (authorRole == "Admin")
        {
            return MyList.FirstOrDefault(u => u.Id == id);
        }
        else if (authorRole == "Author")
        {
            int userId = user.Id;
            if (id == userId)
            {
                return MyList.FirstOrDefault(u => u.Id == id);
            }
        }

        return null; // Unauthorized
    }

    public override bool Update(int id, Author author)
    {
        var authorRole = user.Role;

        if (authorRole == "Admin")
        {
            return UpdateAuthor(id, author);
        }
        else if (authorRole == "Author")
        {
            int userId = user.Id;
            
            if (id == userId)
            {
                return UpdateAuthor(id, author);
            }
        }

        return false; // Unauthorized
    }
     public  bool UpdateAuthor(int id, Author author)
    {
        
       
        if (author == null || (author.role != Role.Admin && author.Id != id) ||
               string.IsNullOrWhiteSpace(author.Name) ||
               string.IsNullOrWhiteSpace(author.Address) ||
             author.BirthDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now.Date)
        {
            System.Console.WriteLine("אין אפשרות לעדכן נתונים שגויים");
            return false;
        }

        System.Console.WriteLine("update function called 2");
        var currentUser = MyList.FirstOrDefault(u => u.Id == id);
        if (currentUser == null)
            return false;

        currentUser.Name = author.Name;
        currentUser.Address = author.Address;
        currentUser.role = author.role;
        currentUser.BirthDate = author.BirthDate;
        saveToFile();
        return true;
        
    }

   

   

    public override int Insert(Author newUser)
    {
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

    public override bool Delete(int id)
    {
        string authorRole = user.Role;
        if(authorRole == "Admin")
        {
            var author = MyList.FirstOrDefault(u => u.Id == id);
            if(author != null)
            {
                MyList.Remove(author);
                deleteAuthorsItem(author.Name);
                saveToFile();
                return true;
            }
        }
        return false; // Unauthorized
    }
    public void deleteAuthorsItem(string authorName){
        System.Console.WriteLine("deleteAuthorsItem function called");
        var authorsBooks = bookService.Get();
        authorsBooks.RemoveAll(b => b.Author == authorName && bookService.Delete(b.Id));
        
    }
}
