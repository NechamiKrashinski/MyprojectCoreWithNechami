using project.Models;

namespace project.Services;

public class AuthorServiceJson : ServiceJson<Author>
{


    public AuthorServiceJson(IHostEnvironment env) : base(env)
    {

    }
    public override List<Author> Get()
    {
        System.Console.WriteLine("mylist is null?" + MyList == null);
        return MyList;
    }

    public override Author Get(int id)
    {
        throw new NotImplementedException();
    }

    public override int Insert(Author newUser)
    {
        if (newUser == null)
        {

            Console.WriteLine("Error: newUser is null.");

            return -1;
        }

        if (string.IsNullOrWhiteSpace(newUser.Name))
        {
            Console.WriteLine("Error: Name is required.");

            return -1;
        }

        if (string.IsNullOrWhiteSpace(newUser.Address))
        {

            Console.WriteLine("Error: Address is required.");

            return -1;
        }

        if (newUser.BirthDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now)
        {

            Console.WriteLine("Error: BirthDate must be in the past.");
            return -1;
        }
        System.Console.WriteLine("inset 222222");

        int maxId = MyList.Any() ? MyList.Max(u => u.Id) : 0;
        System.Console.WriteLine("inset 3333");


        newUser.Id = maxId + 1;
        MyList.Add(newUser);
        saveToFile();

        System.Console.WriteLine("inset 4444");


        return newUser.Id;
    }




    public override bool Update(int id, Author auther)
    {
        System.Console.WriteLine(auther.role);
        System.Console.WriteLine("update function called");
        System.Console.WriteLine(auther.BirthDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now.Date);
        if (auther == null || (auther.role != Role.Admin && auther.Id != id) ||
               string.IsNullOrWhiteSpace(auther.Name) ||
               string.IsNullOrWhiteSpace(auther.Address) ||
             auther.BirthDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now.Date)
        {
            System.Console.WriteLine("אין אפשרות לעדכן נתונים שגויים");
            return false;
        }

        System.Console.WriteLine("update function called 2");
        var currentUser = MyList.FirstOrDefault(u => u.Id == id);
        if (currentUser == null)
            return false;

        currentUser.Name = auther.Name;
        currentUser.Address = auther.Address;
        currentUser.role = auther.role;
        currentUser.BirthDate = auther.BirthDate;
        saveToFile();
        return true;

    }




}
