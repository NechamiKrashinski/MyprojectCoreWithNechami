using project.Interfaces;

namespace project.Models;

public static class CurrentUser
{
    public static int Id { get; set; } = -1; // ערך ברירת מחדל
    public static Role role { get; set; } = Role.Reader; // ערך ברירת מחדל

    // החזרת הערכים הנוכחיים
    // public static (int Id, Role Role) GetCurrentUser()
    // {
    //     return (Id, role);
    // }

    // מימוש השיטה כך שהיא מקבלת את סוג CurrentUser
    public static void SetCurrentUser(int id, Role role2)
    {
        Id = id;
        role = role2;
        System.Console.WriteLine(id+"=======================================");
    }

    // שימוש במילת המפתח new
    public static new string ToString()
    {
        return $"CurrentUser: Id = {Id}, Role = {role}";
    }
}
