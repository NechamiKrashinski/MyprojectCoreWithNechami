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
    protected UserAuth userauth;

    private string _token;

    public string Token
    {
        get => _token;
        set
        {
            _token = value;
            userauth = TokenService.GetUserAuth(_token);
            Console.WriteLine($"Token set: {_token}");
        }
    }

    private readonly AuthorServiceJson authorService;

    public BookServiceJson(IHostEnvironment env, IService<Author> authorService)
        : base(env)
    {
        this.authorService = (AuthorServiceJson)authorService;
        Console.WriteLine("BookServiceJson initialized");
    }

    public override List<Book> Get()
    {
        Console.WriteLine("Get() called");
        Console.WriteLine("Get() called" + MyList.Count + "===" + MyList.ToString());
        if (userauth.role == Role.Author)
        {
            var result = MyList.Where(a => userauth.Id == a.AuthorId).ToList();
            Console.WriteLine($"Returning {result.Count} books for Author role");
            return result;
        }
        else if (userauth.role == Role.Admin)
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

        if (userauth.role == Role.Admin || (book != null && userauth.Id == book.AuthorId))
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
        if (
            newBook == null
            || string.IsNullOrWhiteSpace(newBook.Name)
            || newBook.Price <= 0
            || newBook.AuthorId != userauth.Id && userauth.role != Role.Admin
        )
        {
            Console.WriteLine("Insert failed: Invalid book data");
            return -1;
        }

        var authorName = authorService.Get(newBook.AuthorId)?.Name;
        if (authorName == null)
        {
            Console.WriteLine("Insert failed: Author not found");
            return -1;
        }

        newBook.Author = authorName;
        int maxId = MyList.Any() ? MyList.Max(b => b.Id) : 0;
        newBook.Id = maxId + 1;
        MyList.Add(newBook);
        saveToFile();
        Console.WriteLine($"Book inserted with Id: {newBook.Id}");
        return newBook.Id;
    }

    public bool Update(int id, Book book)
    {
        Console.WriteLine($"Update({id}) called");
        if (
            book == null
            || book.Id != id
            || string.IsNullOrWhiteSpace(book.Name)
            || book.Price <= 0
            || book.AuthorId != userauth.Id && userauth.role != Role.Admin
        )
        {
            Console.WriteLine("Update failed: Invalid book data");
            return false;
        }

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
        if (id != userauth.Id && userauth.role != Role.Admin)
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

    protected void saveToFile()
    {
        Console.WriteLine("Saving books to file");
        File.WriteAllText("books.json", JsonSerializer.Serialize(MyList));
    }
}
