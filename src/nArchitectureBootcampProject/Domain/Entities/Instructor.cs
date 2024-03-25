using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Instructor : User
{
    public string CompanyName { get; set; }

    public virtual ICollection<InstructorImage> InstructorImages { get; set; }

    public Instructor()
    {
        InstructorImages = new HashSet<InstructorImage>();
    }

    public Instructor(string companyName)
    {
        CompanyName = companyName;
    }
}
