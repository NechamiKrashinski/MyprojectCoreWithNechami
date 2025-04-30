using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;

namespace project.Services;

public class BookServiceJson : ServiceJson<Book>
{
    private readonly IService<Author> authorService;

    public BookServiceJson(IHostEnvironment env, IService<Author> authorService)
        : base(env)
    {
        this.authorService = authorService;
    }

    public override int Insert(Book newBook)
    {
        if (newBook == null || string.IsNullOrWhiteSpace(newBook.Name) || newBook.Price <= 0)
            return -1;

        if (authorService.Get()?.Find(u => u.Name == newBook.Author) != null)
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
