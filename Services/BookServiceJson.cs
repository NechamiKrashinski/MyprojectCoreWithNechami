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


    private readonly IService<Author> autherService;

    public BookServiceJson(IHostEnvironment env, IService<Author> autherService) : base(env)
    {
        this.autherService = autherService;
    }

    public override List<Book> Get()
    {
        return MyList;
    }
    public override int Insert(Book newBook)
    {
        if (newBook == null || string.IsNullOrWhiteSpace(newBook.Name) || newBook.Price <= 0)
            return -1;

        if (autherService.Get()?.Find(u => u.Name == newBook.Author) != null)
        {
            int maxId = MyList.Any() ? MyList.Max(b => b.Id) : 0; // Ensure there are books
            newBook.Id = maxId + 1;
            MyList.Add(newBook);
            saveToFile();
            return newBook.Id;
        }
        else
        {
            return -1; // Author not found
        }

    }

    public override bool Update(int id, Book book)
    {
        if (book == null || book.Id != id || string.IsNullOrWhiteSpace(book.Name) || book.Price <= 0)
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
