namespace Domain.Entities;

public class Employee : User
{
    public string Position { get; set; }

    public Employee() { }

    public Employee(string position)
    {
        Position = position;
    }
}
