using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using project.Interfaces;
using project.Models;

namespace project.Services;

public class BookServiceJson : GetFuncService<Book>, IService<Book>
{
    protected ILogin<CurrentUser> currentUser;

  //  private string _token;

    // public string Token
    // {
    //     get => _token;
    //     set
    //     {
    //         _token = value;
    //         currentUser = TokenService.GetCurrentUser(_token);
    //         Console.WriteLine($"Token set: {_token}");
    //     }
    // }

    private readonly AuthorServiceJson authorService;

    public BookServiceJson(IHostEnvironment env, IService<Author> authorService,ILogin<CurrentUser> user)
        : base(env)
    {
        this.authorService = (AuthorServiceJson)authorService;
        this.currentUser = user;
        System.Console.WriteLine(authorService.ToString() + "===" + authorService.ToString());
        Console.WriteLine("BookServiceJson initialized");
    }

    public override List<Book> Get()
    {
        Console.WriteLine("Get() called");
        Console.WriteLine("Get() called" + MyList.Count + "===" + MyList.ToString());
        if (currentUser.role == Role.Author)
        {
            var result = MyList.Where(a => currentUser.Id == a.AuthorId).ToList();
            Console.WriteLine($"Returning {result.Count} books for Author role");
            return result;
        }
        else if (currentUser.role == Role.Admin)
        {
            Console.WriteLine("Returning all books for Admin role");
            return MyList;
        }
        Console.WriteLine("No books found");
        return new List<Book>();
    }

    public Book Get(int id)
    {
        Console.WriteLine($"Get({id}) called");
        Book? book = MyList.FirstOrDefault(b => b.Id == id);

        if (currentUser.role == Role.Admin || (book != null && currentUser.Id == book.AuthorId))
        {
            Console.WriteLine($"Returning book with Id: {id}");
            return book;
        }
        Console.WriteLine("Book not found or unauthorized access");
        return null;
    }

    public int Insert(Book newBook)
    {
        Console.WriteLine("Insert() called");
        try
        {
            // בדיקת נתוני הספר
            if (newBook == null)
            {
                Console.WriteLine("Insert failed: newBook is null");
                return -1;
            }
            if (string.IsNullOrWhiteSpace(newBook.Name))
            {
                Console.WriteLine("Insert failed: Book name is null or whitespace");
                return -1;
            }
            if (newBook.Price <= 0)
            {
                Console.WriteLine("Insert failed: Book price must be greater than zero");
                return -1;
            }
            if (newBook.Date == default) // בדיקת תאריך
            {
                Console.WriteLine("Insert failed: Book date is not set");
                return -1;
            }

            Console.WriteLine(
                $"New book details: Id: {newBook.Id}, Name: {newBook.Name}, Author: {newBook.Author}, AuthorId: {newBook.AuthorId}, Price: {newBook.Price}, Date: {newBook.Date}"
            );
            if (authorService == null)
            {
                Console.WriteLine("Insert failed: authorService is null");
                return -1;
            }

            var authorId = authorService.Id(newBook.Author);
            Console.WriteLine($"Author ID: {authorId} | User ID: {currentUser.Id}");

            // בדיקת הרשאות
            if (authorId != currentUser.Id && currentUser.role != Role.Admin)
            {
                Console.WriteLine("Insert failed: Author not found or user is not authorized");
                return -1;
            }

            // הגדרת AuthorId לספר
            newBook.AuthorId = authorId;
            int maxId = MyList.Any() ? MyList.Max(b => b.Id) : 0;
            newBook.Id = maxId + 1;

            // הוספת הספר לרשימה ושמירה לקובץ
            MyList.Add(newBook);
            saveToFile();
            Console.WriteLine($"Book inserted successfully with Id: {newBook.Id}");

            return newBook.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return -1;
        }
    }

    public bool Update(int id, Book book)
    {
        Console.WriteLine($"Update({id}) called");
        Console.WriteLine(book.ToString() + "service");
        if (
            book == null
            || book.Id != id
            || string.IsNullOrWhiteSpace(book.Name)
            || book.Price <= 0
        )
        {
            Console.WriteLine("Update failed: Invalid book data");
            return false;
        }
        Console.WriteLine(book.ToString() + "service2");
        var authorId = authorService.Id(book.Author);

        if (authorId == null || (authorId != currentUser.Id && currentUser.role != Role.Admin))
        {
            Console.WriteLine("Insert failed: Author not found");
            return false;
        }

        // book.AuthorId = authorId.Value;

        var currentBook = MyList.FirstOrDefault(b => b.Id == id);
        if (currentBook == null)
        {
            Console.WriteLine("Update failed: Book not found");
            return false;
        }

        currentBook.Name = book.Name;
        currentBook.Price = book.Price;
        saveToFile();
        Console.WriteLine("Book updated successfully");
        return true;
    }

    public bool Delete(int id)
    {
        Console.WriteLine($"Delete({id}) called");
        var authorId = Get(id)?.AuthorId;
        if (authorId == null)
            return false;
        if (authorId != currentUser.Id && currentUser.role != Role.Admin)
        {
            Console.WriteLine("Delete failed: Unauthorized access");
            return false;
        }
        var currentT = MyList.FirstOrDefault(b => b.Id == id);
        if (currentT == null)
        {
            Console.WriteLine("Delete failed: Book not found");
            return false;
        }

        MyList.Remove(currentT);
        saveToFile();
        Console.WriteLine("Book deleted successfully");
        return true;
    }
}
