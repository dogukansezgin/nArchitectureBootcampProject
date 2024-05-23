using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IInstructorRepository : IAsyncRepository<Instructor, Guid>, IRepository<Instructor, Guid>
{
    Task<Instructor> RestoreAsync(Instructor instructor);
    Task<ICollection<Instructor>> DeleteRangeCustomAsync(
        ICollection<Instructor> entities,
        bool permanent = false,
        CancellationToken cancellationToken = default
    );
    Task<ICollection<Instructor>> RestoreRangeCustomAsync(ICollection<Instructor> entities);
}
