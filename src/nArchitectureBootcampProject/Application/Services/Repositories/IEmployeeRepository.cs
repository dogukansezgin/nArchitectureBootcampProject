using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IEmployeeRepository : IAsyncRepository<Employee, Guid>, IRepository<Employee, Guid> 
{
    Task<Employee> RestoreAsync(Employee employee);
    Task<ICollection<Employee>> DeleteRangeCustomAsync(
        ICollection<Employee> entities,
        bool permanent = false,
        CancellationToken cancellationToken = default
    );
    Task<ICollection<Employee>> RestoreRangeCustomAsync(ICollection<Employee> entities);
}
