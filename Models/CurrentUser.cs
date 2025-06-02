
namespace project.Models;

public static class CurrentUser
{
    public static int Id { get; set; } = -1; // ערך ברירת מחדל
    public static Role role { get; set; } = Role.Reader; // ערך ברירת מחדל


    public static void SetCurrentUser(int id, Role role2)
    {
        Id = id;
        role = role2;
    }

    // שימוש במילת המפתח new
    public static new string ToString()
    {
        return $"CurrentUser: Id = {Id}, Role = {role}";
    }
}
