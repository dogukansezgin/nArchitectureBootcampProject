using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IBootcampRepository : IAsyncRepository<Bootcamp, Guid>, IRepository<Bootcamp, Guid>
{
    Task<Bootcamp> RestoreAsync(Bootcamp bootcamp);
    Task<ICollection<Bootcamp>> DeleteRangeCustomAsync(
        ICollection<Bootcamp> entities,
        bool permanent = false,
        CancellationToken cancellationToken = default
    );
    Task<ICollection<Bootcamp>> RestoreRangeCustomAsync(ICollection<Bootcamp> entities);
}
