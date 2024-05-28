using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IBlacklistRepository : IAsyncRepository<Blacklist, Guid>, IRepository<Blacklist, Guid> 
{
    Task<Blacklist> RestoreAsync(Blacklist blacklist);
    Task<ICollection<Blacklist>> DeleteRangeCustomAsync(
        ICollection<Blacklist> entities,
        bool permanent = false,
        CancellationToken cancellationToken = default
    );
    Task<ICollection<Blacklist>> RestoreRangeCustomAsync(ICollection<Blacklist> entities);
}
