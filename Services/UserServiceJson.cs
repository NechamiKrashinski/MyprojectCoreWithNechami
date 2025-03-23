namespace project.Services;

public class UserServiceJson : ServiceJson{

     

public UserServiceJson(){
    ServiceJson.fileName="user.json";
    base();
}
    public  abstract Insert(User newUser){
          if(newUser == null ||  String.IsNullOrWhiteSpace(newUser.Name)||  String.IsNullOrWhiteSpace(newUser.address) || newUser.BirthDate > DateTime.now() )
            return-1;
       
         int MaxId = ListUsers.Max(b=> b.Id);
         newUser.Id = MaxId+1;
         ListUsers.Add(newUser);
         saveToFile();
         return newUser.Id;
    }

    public abstract bool Update(int id ,T user){
         if(user == null || user.Id!=id|| string.IsNullOrWhiteSpace(user.Name)|| string.IsNullOrWhiteSpace(user.address) || || newUser.BirthDate > DateTime.now())
            return false;

        var currentUser= ListUsers.FirstOrDefault(b=> b.Id==id);
        if(currentUser == null)
            return false;
        
        currentUser.Name = user.Name;
        currentUser.address = user.address;
        currentUser.BirthDate=user.BirthDate;
        saveToFile();
        return true;
    }
   
}