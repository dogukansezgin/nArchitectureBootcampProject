using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class ApplicationState : Entity<Guid>
{
    public string Name { get; set; }

    public ApplicationState() { }

    public ApplicationState(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
