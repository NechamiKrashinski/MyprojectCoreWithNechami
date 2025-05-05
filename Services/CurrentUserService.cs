

using System.Security.Claims;

public class CurrentUserService : User
 {
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
 
      
        
    public User GetCurrentUser()
    {
         var user = _httpContextAccessor.HttpContext?.User;

        if (user != null && user.Identity.IsAuthenticated)
        {
            
       
            var userId =  user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName =  user.FindFirst(ClaimTypes.Name)?.Value;
            var role =  user.FindFirst(ClaimTypes.Role)?.Value;
            
            Id = int.Parse(userId);
            Name = userName;
            Role = role;

           
        }

        return null;
    }
    

}
