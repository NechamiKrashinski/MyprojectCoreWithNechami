using project.Models;

namespace project.Services;

public class UserServiceJson : ServiceJson<Author>
{
<<<<<<< HEAD

    public UserServiceJson(IHostEnvironment env) : base(env)
    {

    }
    public override List<Author> Get()
    {
        return MyList;
    }
=======
    public UserServiceJson(IHostEnvironment env)
        : base(env) { }

>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
    public override int Insert(Author newUser)
    {
        if (newUser == null)
        {
<<<<<<< HEAD
            Console.WriteLine("Error: newUser is null.");
=======
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
            return -1;
        }

        if (string.IsNullOrWhiteSpace(newUser.Name))
        {
<<<<<<< HEAD
            Console.WriteLine("Error: Name is required.");
=======
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
            return -1;
        }

        if (string.IsNullOrWhiteSpace(newUser.Address))
        {
<<<<<<< HEAD
            Console.WriteLine("Error: Address is required.");
=======
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
            return -1;
        }

        if (newUser.BirthDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
        {
<<<<<<< HEAD
            Console.WriteLine("Error: BirthDate must be in the past.");
            return -1;
        }
        System.Console.WriteLine("inset 222222");

        int maxId = MyList.Any() ? MyList.Max(u => u.Id) : 0;
        System.Console.WriteLine("inset 3333");
=======
            return -1;
        }

        int maxId = MyList.Any() ? MyList.Max(u => u.Id) : 0;
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa

        newUser.Id = maxId + 1;
        MyList.Add(newUser);
        saveToFile();
<<<<<<< HEAD
        System.Console.WriteLine("inset 4444");
=======
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa

        return newUser.Id;
    }

<<<<<<< HEAD


    public override bool Update(int id, Author auther)
    {

        if (auther == null || auther.Id != id ||
               string.IsNullOrWhiteSpace(auther.Name) ||
               string.IsNullOrWhiteSpace(auther.Address) ||
              auther.BirthDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now)
=======
    public override bool Update(int id, Author author)
    {
        if (
            author == null
            || author.Id != id
            || string.IsNullOrWhiteSpace(author.Name)
            || string.IsNullOrWhiteSpace(author.Address)
            || author.BirthDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now
        )
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
            return false;

        var currentUser = MyList.FirstOrDefault(u => u.Id == id);
        if (currentUser == null)
            return false;

<<<<<<< HEAD
        currentUser.Name = auther.Name;
        currentUser.Address = auther.Address;
        currentUser.BirthDate = auther.BirthDate;
        saveToFile();
        return true;

    }



=======
        currentUser.Name = author.Name;
        currentUser.Address = author.Address;
        currentUser.BirthDate = author.BirthDate;
        saveToFile();
        return true;
    }
>>>>>>> e5f0c2f45f3159d29c8be38a0b4d2eeb1432a9fa
}
