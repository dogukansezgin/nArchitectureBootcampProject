using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Services.Repositories;

public interface IApplicationRepository : IAsyncRepository<ApplicationEntity, Guid>, IRepository<ApplicationEntity, Guid>
{
    Task<ApplicationEntity> RestoreAsync(ApplicationEntity application);
    Task<ICollection<ApplicationEntity>> DeleteRangeCustomAsync(
        ICollection<ApplicationEntity> entities,
        bool permanent = false,
        CancellationToken cancellationToken = default
    );
    Task<ICollection<ApplicationEntity>> RestoreRangeCustomAsync(ICollection<ApplicationEntity> entities);
}
