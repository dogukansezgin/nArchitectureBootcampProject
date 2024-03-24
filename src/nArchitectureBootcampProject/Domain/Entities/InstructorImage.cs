using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class InstructorImage : Entity<Guid>
{
    public Guid InstructorId { get; set; }
    public string ImagePath { get; set; }

    public virtual Instructor Instructor { get; set; }

    public InstructorImage()
    {
        
    }

    public InstructorImage(Guid id, Guid instructorId, string imagePath)
    {
        Id = id;
        InstructorId = instructorId;
        ImagePath = imagePath;
    }
}
