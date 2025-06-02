using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using project.Interfaces;
using project.Models;

namespace project.Services;

public class ItemServiceJson<T> : GetFuncService<T>, IItemService<T>
    where T : IItem
{
    private readonly IUserService<Author> authorService;

    private readonly int userId;
    private readonly Role role;

    public ItemServiceJson(IHostEnvironment env, IUserService<Author> authorService)
        : base(env)
    {
        this.authorService = authorService;
        userId = CurrentUser.Id;
        role = CurrentUser.role;
    }

    public override List<T> Get()
    {
        System.Console.WriteLine(
            "------Get method called " + role.ToString() + " " + userId.ToString()
        );
        if (role == Role.Author)
        {
            System.Console.WriteLine(MyList.Count + "===" + MyList.ToString());

            var filteredItems = MyList.Where(a => userId == a.UserId);
            Console.WriteLine("Filtered items count: " + filteredItems.Count());
            var result = filteredItems.ToList();
            return result;
        }
        else if (role == Role.Admin)
        {
            return MyList;
        }
        throw new Exception("Unauthorized access");
    }

    public T Get(int id)
    {
        T? item = MyList.FirstOrDefault(b => b.Id == id);

        if (role == Role.Admin || (item != null && userId == item.UserId))
        {
            if (item == null)
            {
                throw new Exception("Item not found");
            }
            return item;
        }
        throw new Exception("Unauthorized access");
    }

    public int Insert(T newItem)
    {
        try
        {
            if (
                newItem == null
                || string.IsNullOrWhiteSpace(newItem.Name)
                || newItem.Date == default
                || authorService == null
                || (role != Role.Admin && newItem.UserId != userId)
            )
            {
                Console.WriteLine("Insert failed: invalid input or permissions");
                throw new Exception("Insert failed: invalid input or permissions");
            }

            newItem.UserId = role == Role.Admin ? newItem.UserId : userId;
            newItem.Id = (MyList.Any() ? MyList.Max(b => b.Id) : 0) + 1;

            MyList.Add(newItem);
            saveToFile();
            Console.WriteLine($"Item inserted successfully with Id: {newItem.Id}");

            return newItem.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw new Exception("Insert failed: " + ex.Message);
        }
    }

    public bool Update(int id, T item)
    {
        Console.WriteLine($"Update({id}) called");
        Console.WriteLine(item.ToString() + "service");
        if (item == null || item.Id != id || string.IsNullOrWhiteSpace(item.Name))
        {
            Console.WriteLine("Update failed: Invalid item data");
            throw new Exception("Update failed: Invalid item data");
        }
        Console.WriteLine(item.ToString() + "service2");

        if (item.UserId != userId && role != Role.Admin)
        {
            Console.WriteLine("Insert failed: Author not found");
            throw new Exception("Insert failed: Author not found");
        }

        // item.UserId = userId.Value;

        var currentItem = MyList.FirstOrDefault(b => b.Id == id);
        if (currentItem == null)
        {
            Console.WriteLine("Update failed: Item not found");
            throw new Exception("Update failed: Item not found");
        }

        currentItem.Name = item.Name;
        saveToFile();
        Console.WriteLine("Item updated successfully");
        return true;
    }

    public bool Delete(int id)
    {
        Console.WriteLine($"Delete({id}) called");
        var userId = Get(id)?.UserId;
        var currentT = MyList.FirstOrDefault(b => b.Id == id);

        if (userId == null || (userId != this.userId && role != Role.Admin) || currentT == null)
        {
            Console.WriteLine("Delete failed: Unauthorized access or Item not found");
            throw new Exception("Delete failed: Unauthorized access or Item not found");
        }

        MyList.Remove(currentT);
        saveToFile();
        Console.WriteLine("Item deleted successfully");
        return true;
    }
}
