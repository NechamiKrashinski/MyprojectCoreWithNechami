using project.Interfaces;

public interface IItem
{
    int Id { get; set; }
    string Name { get; set; }
    DateTime Date { get; set; }
    int UserId { get; set; }
}
