using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class BootcampImage : Entity<Guid>
{
    public Guid BootcampId { get; set; }
    public string ImagePath { get; set; }

    public virtual Bootcamp Bootcamp { get; set; }

    public BootcampImage() { }

    public BootcampImage(Guid id, Guid bootcampId, string ımagePath)
    {
        Id = id;
        BootcampId = bootcampId;
        ImagePath = ımagePath;
    }
}
