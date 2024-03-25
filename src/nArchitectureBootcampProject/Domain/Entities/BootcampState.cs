using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class BootcampState : Entity<Guid>
{
    public string Name { get; set; }

    public BootcampState() { }

    public BootcampState(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
