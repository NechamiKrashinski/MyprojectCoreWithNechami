using Microsoft.AspNetCore.Mvc;


using project.Models;

namespace project.Interfaces;


public interface IBookService
{
   
    List<Book> Get();

    Book Get(int id);

    int Insert(Book newBook);
    
    bool Update(int id ,Book book);
   
   bool Delete(int id);
   

}