// using project.Interfaces;
// using project.Models;

// namespace project.Services;

// public class AuthorServiceJson : ServiceJson<Author>
// {
//     public AuthorServiceJson(IHostEnvironment env)
//         : base(env) { }

//     public override int Insert(Author newUser)
//     {
//         if (newUser == null)
//         {
//             return -1;
//         }

//         if (string.IsNullOrWhiteSpace(newUser.Name))
//         {
//             return -1;
//         }

//         if (string.IsNullOrWhiteSpace(newUser.Address))
//         {
//             return -1;
//         }

//         if (newUser.BirthDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
//         {
//             return -1;
//         }

//         int maxId = MyList.Any() ? MyList.Max(u => u.Id) : 0;

//         newUser.Id = maxId + 1;
//         MyList.Add(newUser);
//         saveToFile();

//         return newUser.Id;
//     }

//     public override bool Update(int id, Author author)
//     {
//         Console.WriteLine($"Update called with id: {id}");
//         Console.WriteLine($"Author provided: {author}");

//         if (author == null)
//         {
//             Console.WriteLine("Author is null.");
//             return false;
//         }

//         if (author.Id != id)
//         {
//             Console.WriteLine("Author ID does not match.");
//             return false;
//         }

//         if (string.IsNullOrWhiteSpace(author.Name))
//         {
//             Console.WriteLine("Author name is empty or whitespace.");
//             return false;
//         }

//         if (string.IsNullOrWhiteSpace(author.Address))
//         {
//             Console.WriteLine("Author address is empty or whitespace.");
//             return false;
//         }

//         // if (author.BirthDate.ToDateTime(TimeOnly.MinValue).Date <= DateTime.Today)
//         // {
//         //     Console.WriteLine("Author birth date is not valid.");
//         //     return false;
//         // }

//         Console.WriteLine("Validation succeeded.");

//         var currentUser = MyList.FirstOrDefault(u => u.Id == id);
//         if (currentUser == null)
//         {
//             Console.WriteLine("Current user not found.");
//             return false;
//         }

//         Console.WriteLine($"Updating user: {currentUser.Name}");
//         currentUser.Name = author.Name;
//         currentUser.Address = author.Address;
//         currentUser.BirthDate = author.BirthDate;
//         saveToFile();
//         Console.WriteLine("Update successful.");
//         return true;
//     }
// }using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using project.Interfaces;
using project.Models;

namespace project.Services;

public class AuthorServiceJson : GetFuncService<Author>, IService<Author>
{
    private string _token;

    //private List<Author> MyList = new List<Author>(); // רשימה לאחסון מחברים
    protected UserAuth userauth;

    public AuthorServiceJson(IHostEnvironment env)
        : base(env) { }

    public string Token
    {
        get => _token;
        set
        {
            _token = value;
            userauth = TokenService.GetUserAuth(_token);
        }
    }

    public override List<Author> Get()
    {
        if (userauth.role == Role.Author)
        {
            return new List<Author> { Get(userauth.Id) }
                .Where(a => a != null)
                .ToList();
        }
        else if (userauth.role == Role.Admin)
        {
            return MyList;
        }
        return new List<Author>();
    }

    public Author Get(int id)
    {
        var author= MyList.FirstOrDefault(b => b.Id == id);
        if (author == null)
        {
            return null;
        }
        return author;

    }

    public int Insert(Author newUser)
    {
        if (
            newUser == null
            || string.IsNullOrWhiteSpace(newUser.Name)
            || string.IsNullOrWhiteSpace(newUser.Address)
        )
        {
            return -1;
        }

        if (newUser.BirthDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
        {
            return -1;
        }

        int maxId = MyList.Any() ? MyList.Max(u => u.Id) : 0;
        newUser.Id = maxId + 1;
        MyList.Add(newUser);
        saveToFile();

        return newUser.Id;
    }

    public bool Update(int id, Author author)
    {
        if (userauth.role == Role.Admin || userauth.role == Role.Author && userauth.Id == id)
        {
            Console.WriteLine($"Update called with id: {id}");
            Console.WriteLine($"Author provided: {author}");

            if (author == null)
            {
                Console.WriteLine("Author is null.");
                return false;
            }

            if (author.Id != id)
            {
                Console.WriteLine("Author ID does not match.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(author.Name))
            {
                Console.WriteLine("Author name is empty or whitespace.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(author.Address))
            {
                Console.WriteLine("Author address is empty or whitespace.");
                return false;
            }

            // if (author.BirthDate.ToDateTime(TimeOnly.MinValue).Date <= DateTime.Today)
            // {
            //     Console.WriteLine("Author birth date is not valid.");
            //     return false;
            // }

            Console.WriteLine("Validation succeeded.");
            {
                if (
                    author == null
                    || author.Id != id
                    || string.IsNullOrWhiteSpace(author.Name)
                    || string.IsNullOrWhiteSpace(author.Address)
                )
                {
                    return false;
                }

                var currentUser = MyList.FirstOrDefault(u => u.Id == id);
                if (currentUser == null)
                {
                    return false;
                }

                currentUser.Name = author.Name;
                currentUser.Address = author.Address;
                currentUser.BirthDate = author.BirthDate;
                saveToFile();
                Console.WriteLine("Update successful.");
                return true;
            }
        }
        return false;
    }

    public bool Delete(int id)
    {
        var currentT = MyList.FirstOrDefault(b => b.Id == id);
        if (currentT == null)
            return false;

        MyList.Remove(currentT);
        saveToFile();
        return true;
    }

    protected void saveToFile()
    {
        File.WriteAllText("authors.json", JsonSerializer.Serialize(MyList));
    }
}
