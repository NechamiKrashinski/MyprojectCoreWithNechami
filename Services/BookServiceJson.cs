using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;

using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;
using project.Models;

namespace project.Services;

public class BookServiceJson : ServiceJson<Book>
{
    private readonly IService<Author> authorService;



   

    public BookServiceJson(IHostEnvironment env, IService<Author> autherS) : base(env)
    {
       
        authorService = autherS; // Incorrect assignment
    }

    public override List<Book> Get()
    {
        
        return MyList;
    }
    public List< Book>? GetAuthorsBook(String authorName)
    {
        System.Console.WriteLine(authorName + " authorName in book service");
        List< Book> books = MyList.FindAll(b => b.Author == authorName);
        if (books != null)
        {
            foreach (var book in books)
            {
                System.Console.WriteLine(book.Name + " book name in book service");
            }
            System.Console.WriteLine("Book found: ");
            return  books ;
        }
        else
        {
            System.Console.WriteLine("Error: Book not found.");
            return null;
        }
    }
    
    public override int Insert(Book newBook)
    {
        System.Console.WriteLine(newBook.Name);
        System.Console.WriteLine(newBook.Price);
        System.Console.WriteLine(newBook.Author);
        System.Console.WriteLine(newBook.Date);
        System.Console.WriteLine(newBook.Id);
      
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

        
        if (authorService.Get().FirstOrDefault(u => u.Name == newBook.Author) != null)
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
            System.Console.WriteLine("Error: Author does not exist in the list.");
            return -1;
        }
    }

    public override bool Update(int id, Book book)
    {
        if (
            book == null
            || book.Id != id
            || string.IsNullOrWhiteSpace(book.Name)
            || book.Price <= 0
        )
            return false;
        System.Console.WriteLine("Update book: " + book.Name);
        var currentBook = MyList.FirstOrDefault(b => b.Id == id);
        if (currentBook == null)
        {
            System.Console.WriteLine("Error: Book not found.");
             return false;
        }
       
        currentBook.Name = book.Name;
        currentBook.Price = book.Price;
        //עדכון של השם רק אם הוא קיים ברשימה של הסופרים
        if( authorService.Get().FirstOrDefault(u => u.Name == book.Author)!= null)
        {
             currentBook.Author = book.Author;
        }
        else
        {
            System.Console.WriteLine("Error: Author does not exist in the list.");
            return false;
        }
        
        currentBook.Date = book.Date;
        saveToFile();
        return true;
    }
}
