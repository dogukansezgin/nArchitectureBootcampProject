using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Services.Bootcamps;

public interface IBootcampService
{
    Task<Bootcamp?> GetAsync(
        Expression<Func<Bootcamp, bool>> predicate,
        Func<IQueryable<Bootcamp>, IIncludableQueryable<Bootcamp, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Bootcamp>?> GetListAsync(
        Expression<Func<Bootcamp, bool>>? predicate = null,
        Func<IQueryable<Bootcamp>, IOrderedQueryable<Bootcamp>>? orderBy = null,
        Func<IQueryable<Bootcamp>, IIncludableQueryable<Bootcamp, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Bootcamp> AddAsync(Bootcamp bootcamp);
    Task<Bootcamp> UpdateAsync(Bootcamp bootcamp);
    Task<Bootcamp> DeleteAsync(Bootcamp bootcamp, bool permanent = false);
    Task<ICollection<Bootcamp>> DeleteRangeAsync(ICollection<Bootcamp> bootcamps, bool permanent = false);
    Task<Bootcamp> RestoreAsync(Bootcamp bootcamp);
    Task<ICollection<Bootcamp>> RestoreRangeAsync(ICollection<Bootcamp> bootcamps);
    Task<Bootcamp> GetByIdAsync(Guid id);
    Task<Bootcamp> GetByNameAsync(string name);
}
