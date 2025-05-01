using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;
using project.Models;
=======

>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
namespace project.Services;

public class BookServiceJson : ServiceJson<Book>
{
    private readonly IService<Author> authorService;

<<<<<<< HEAD

    private readonly IService<Author> autherService;

    public BookServiceJson(IHostEnvironment env, IService<Author> autherService) : base(env)
=======
    public BookServiceJson(IHostEnvironment env, IService<Author> authorService)
        : base(env)
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
    {
        this.authorService = authorService;
    }

    public override List<Book> Get()
    {
        return MyList;
    }
    public override int Insert(Book newBook)
    {
        if (newBook == null || string.IsNullOrWhiteSpace(newBook.Name) || newBook.Price <= 0)
            return -1;

<<<<<<< HEAD
        if (autherService.Get()?.Find(u => u.Name == newBook.Author) != null)
=======
        if (authorService.Get()?.Find(u => u.Name == newBook.Author) != null)
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
        {
            int maxId = MyList.Any() ? MyList.Max(b => b.Id) : 0; // Ensure there are books
            newBook.Id = maxId + 1;
            MyList.Add(newBook);
            saveToFile();
            return newBook.Id;
        }
        else
        {
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

        var currentBook = MyList.FirstOrDefault(b => b.Id == id);
        if (currentBook == null)
            return false;

        currentBook.Name = book.Name;
        currentBook.Price = book.Price;
        saveToFile();
        return true;
    }
}
