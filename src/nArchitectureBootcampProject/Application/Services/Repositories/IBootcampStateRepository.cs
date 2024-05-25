using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IBootcampStateRepository : IAsyncRepository<BootcampState, Guid>, IRepository<BootcampState, Guid> 
{
    Task<BootcampState> RestoreAsync(BootcampState bootcampState);
    Task<ICollection<BootcampState>> DeleteRangeCustomAsync(
        ICollection<BootcampState> entities,
        bool permanent = false,
        CancellationToken cancellationToken = default
    );
    Task<ICollection<BootcampState>> RestoreRangeCustomAsync(ICollection<BootcampState> entities);
}
