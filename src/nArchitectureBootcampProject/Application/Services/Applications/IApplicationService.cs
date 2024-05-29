using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Services.Applications;

public interface IApplicationService
{
    Task<ApplicationEntity?> GetAsync(
        Expression<Func<ApplicationEntity, bool>> predicate,
        Func<IQueryable<ApplicationEntity>, IIncludableQueryable<ApplicationEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<ApplicationEntity>?> GetListAsync(
        Expression<Func<ApplicationEntity, bool>>? predicate = null,
        Func<IQueryable<ApplicationEntity>, IOrderedQueryable<ApplicationEntity>>? orderBy = null,
        Func<IQueryable<ApplicationEntity>, IIncludableQueryable<ApplicationEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<ApplicationEntity> AddAsync(ApplicationEntity application);
    Task<ApplicationEntity> UpdateAsync(ApplicationEntity application);
    Task<ICollection<ApplicationEntity>> UpdateRangeAsync(ICollection<ApplicationEntity> applications);
    Task<ApplicationEntity> DeleteAsync(ApplicationEntity application, bool permanent = false);
    Task<ICollection<ApplicationEntity>> DeleteRangeAsync(ICollection<ApplicationEntity> applications, bool permanent = false);
    Task<ApplicationEntity> RestoreAsync(ApplicationEntity application);
    Task<ICollection<ApplicationEntity>> RestoreRangeAsync(ICollection<ApplicationEntity> applications);
    Task<ApplicationEntity> GetByIdAsync(Guid id);
}
