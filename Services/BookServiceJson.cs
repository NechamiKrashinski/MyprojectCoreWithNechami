

using project.Interfaces;
using project.Models;



namespace project.Services;

public class BookServiceJson : ServiceJson<Book>
{
  
    private readonly CurrentUserService user;
   
     



    public BookServiceJson(IHostEnvironment env, CurrentUserService currentUserService) : base(env)
    {

        
        
        this.user = currentUserService;
    }

    public override List<Book> Get()
    {
        var role = user.Role;
        if (role == "Admin")
        {
            System.Console.WriteLine("Admin role in Bookservice");

            return MyList;
        }
        else if (role == "Author")
        {

            System.Console.WriteLine("Author role in Bookservice");
            string name = user.Name;
            return GetAuthorsBook(name) ?? new List<Book>();
        }

        return MyList;
    }
   
    public List<Book>? GetAuthorsBook(String authorName)
    {
        System.Console.WriteLine(authorName + " authorName in book service");
        List<Book> books = MyList.FindAll(b => b.Author == authorName);
        if (books != null)
        {
            foreach (var book in books)
            {
                System.Console.WriteLine(book.Name + " book name in book service");
            }
            System.Console.WriteLine("Book found: ");
            return books;
        }
        else
        {
            System.Console.WriteLine("Error: Book not found.");
            return null;
        }
    }




    public IEnumerable<Book> GetBooksByRole()
    {
        var role = user.Role;
        if (role == "Admin")
        {
            return MyList;
        }
        else if (role == "Author")
        {
            string name = user.Name;
            return GetAuthorsBook(name) ?? new List<Book>();
        }
        return new List<Book>();
    }

    public override Book Get(int id)
    {
        var book = MyList.FirstOrDefault(b => b.Id == id);
        var role = user.Role;

        if (role == "Admin")
        {
            return book ?? throw new ApplicationException("Book not found");
        }
        else if (role == "Author")
        {
            string authorName = user.Name;
            if (book == null || book.Author != authorName)
            {
                throw new ApplicationException("Unauthorized access");
            }
            return book;
        }
        return null;
    }
     

       
    

    public override int Insert(Book newBook)
    {
        if (newBook == null)
        {
            System.Console.WriteLine("Error: Book is null.");
            return -1;
        }
        if (string.IsNullOrWhiteSpace(newBook.Name))
        {
            System.Console.WriteLine("Error: Name is required.");
            return -1;
        }

        if (string.IsNullOrWhiteSpace(newBook.Author))
        {
            System.Console.WriteLine("Error: Author is required.");
            return -1;
        }

        if (newBook.Date.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
        {
            System.Console.WriteLine("Error: Date must be in the past.");
            return -1;
        }

        // if (authorService.Get().FirstOrDefault(u => u.Name == newBook.Author) == null)
        // {

        //     System.Console.WriteLine("Error: Author does not exist in the list.");
        //     return -1;

        // }
        // else
        

            var authorName = user.Name;
            var role = user.Role;

            if (role == "Admin" || (role == "Author" && newBook.Author == authorName))
            {
                System.Console.WriteLine("Author exists in the list.");
                int maxId = MyList.Any() ? MyList.Max(b => b.Id) : 0; // Ensure there are books
                newBook.Id = maxId + 1;
                MyList.Add(newBook);
                saveToFile();
                return newBook.Id;
            }
            else
            {
                return -2; // Unauthorized
            }


        

    }

    public override bool Update(int id, Book book)
    {
        var existingBook = MyList.FirstOrDefault(b => b.Id == id);
        var role = user.Role;
        var authorName = user.Name;

        if (book == null || existingBook == null || role == "Author" && existingBook.Author != authorName)
        {
            return false; // Unauthorized
        }
        if (book.Id != id
            || string.IsNullOrWhiteSpace(book.Name)
            || book.Price <= 0
        )
            return false;



        existingBook.Name = book.Name;
        existingBook.Price = book.Price;
         existingBook.Author = book.Author;
        //עדכון של השם רק אם הוא קיים ברשימה של הסופרים
        // if (authorService.Get().FirstOrDefault(u => u.Name == book.Author) != null)
        // {
        //     existingBook.Author = book.Author;
        // }
        // else
        // {
        //     System.Console.WriteLine("Error: Author does not exist in the list.");
        //     return false;
        // }

        existingBook.Date = book.Date;
        saveToFile();
        return true;


    }

    public override bool Delete(int id)
    {
        var existingBook = MyList.FirstOrDefault(b => b.Id == id);
        var role = user.Role;
        var authorName = user.Name;

        if (existingBook == null || role == "Author" && existingBook.Author != authorName)
        {
            return false; // Unauthorized
        }



        int index = MyList.IndexOf(existingBook);
        MyList.RemoveAt(index);
        saveToFile();
        return true;

    }


}