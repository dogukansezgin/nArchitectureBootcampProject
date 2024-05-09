namespace Domain.Entities;

public class Applicant : User
{
    public string? About { get; set; } = null;

    public Applicant() { }

    public Applicant(string about)
    {
        About = about;
    }
}
