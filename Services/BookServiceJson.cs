using Microsoft.AspNetCore.Mvc;

using project.Interfaces;
using project.Models;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;
namespace project.Services;

public class BookServiceJson : IBookService
{
    List<Book> ListBooks {get;}
    private static string fileName= "book.json";
    private string filePath;

    public BookServiceJson (IHostEnvironment env)
    {
      filePath=Path.Combine(env.ContentRootPath,"data",fileName);
      using(var jsonFile = File.OpenText(filePath)){
          ListBooks=JsonSerializer.Deserialize<List<Book>>(jsonFile.ReadToEnd(),
          new JsonSerializerOptions{
              PropertyNameCaseInsensitive = true
          });
      }
    }

private void saveToFile(){
    File.WriteAllText(filePath,JsonSerializer.Serialize(ListBooks));
}



    public  List<Book> Get(){
        return ListBooks;
    }

     public  Book Get(int id){
        var book= ListBooks.FirstOrDefault(b=> b.Id==id);
        return book;
    }
 public  int Insert(Book newBook)
    {
        if(newBook == null ||  String.IsNullOrWhiteSpace(newBook.Name) || newBook.Price <=0 )
            return-1;
       
         int MaxId = ListBooks.Max(b=> b.Id);
         newBook.Id = MaxId+1;
         ListBooks.Add(newBook);
         saveToFile();
         return newBook.Id;
       
    }

    public bool Update(int id ,Book book)
    {
        if(book == null || book.Id!=id|| string.IsNullOrWhiteSpace(book.Name) || book.Price <=0)
            return false;

        var currentBook= ListBooks.FirstOrDefault(b=> b.Id==id);
        if(currentBook == null)
            return false;
        
        currentBook.Name = book.Name;
        currentBook.Price = book.Price;
        saveToFile();
        return true;
    }

    public bool Delete(int id)
    {
        var currentBook= ListBooks.FirstOrDefault(b=> b.Id==id);
        if(currentBook == null)
            return false;
        
        int index = ListBooks.IndexOf(currentBook);
        ListBooks.RemoveAt(index);
        saveToFile();
        return true;
    }

}