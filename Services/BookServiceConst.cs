using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;

namespace project.Services;

public class BookServiceConst : IService<Book>
{
    private static List<Book> listBooks = new List<Book>();

    public BookServiceConst()
    {
<<<<<<< HEAD
        listBooks = new List<Book>{
            new Book {Id=1, Name = "איסתרק", Author = "מיה קינן", Price = 70, Date= DateOnly.FromDateTime(DateTime.Now.AddYears(-2)) },
            new Book {Id=2, Name = "מהלהלל", Author = "מיה קינן", Price = 70 , Date= DateOnly.FromDateTime(DateTime.Now.AddYears(-2)) }
=======
        listBooks = new List<Book>
        {
            new Book
            {
                Id = 1,
                Name = "איסתרק",
                Author = "מיה קינן",
                Price = 70,
                Date = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),
            },
            new Book
            {
                Id = 2,
                Name = "מהלהלל",
                Author = "מיה קינן",
                Price = 70,
                Date = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),
            },
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
        };
    }

    public List<Book> Get()
    {
        return listBooks;
    }

    public Book Get(int id)
    {
        var book = listBooks.FirstOrDefault(b => b.Id == id);
        return book;
    }

    public int Insert(Book newBook)
    {
        if (newBook == null || String.IsNullOrWhiteSpace(newBook.Name) || newBook.Price <= 0)
            return -1;

        int MaxId = listBooks.Max(b => b.Id);
        newBook.Id = MaxId + 1;
        listBooks.Add(newBook);
        return newBook.Id;
    }

    public bool Update(int id, Book book)
    {
        if (
            book == null
            || book.Id != id
            || string.IsNullOrWhiteSpace(book.Name)
            || book.Price <= 0
        )
            return false;

        var currentBook = listBooks.FirstOrDefault(b => b.Id == id);
        if (currentBook == null)
            return false;

        currentBook.Name = book.Name;
        currentBook.Price = book.Price;
        return true;
    }

    public bool Delete(int id)
    {
        var currentBook = listBooks.FirstOrDefault(b => b.Id == id);
        if (currentBook == null)
            return false;

        int index = listBooks.IndexOf(currentBook);
        listBooks.RemoveAt(index);
        return true;
    }
}
