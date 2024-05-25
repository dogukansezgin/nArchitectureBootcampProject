using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IApplicationStateRepository : IAsyncRepository<ApplicationState, Guid>, IRepository<ApplicationState, Guid> 
{
    Task<ApplicationState> RestoreAsync(ApplicationState applicationState);
    Task<ICollection<ApplicationState>> DeleteRangeCustomAsync(
        ICollection<ApplicationState> entities,
        bool permanent = false,
        CancellationToken cancellationToken = default
    );
    Task<ICollection<ApplicationState>> RestoreRangeCustomAsync(ICollection<ApplicationState> entities);
}
