using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Services.Employees;

public interface IEmployeeService
{
    Task<Employee?> GetAsync(
        Expression<Func<Employee, bool>> predicate,
        Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Employee>?> GetListAsync(
        Expression<Func<Employee, bool>>? predicate = null,
        Func<IQueryable<Employee>, IOrderedQueryable<Employee>>? orderBy = null,
        Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<Employee> DeleteAsync(Employee employee, bool permanent = false);
    Task<ICollection<Employee>> DeleteRangeAsync(ICollection<Employee> employees, bool permanent = false);
    Task<Employee> RestoreAsync(Employee employee);
    Task<ICollection<Employee>> RestoreRangeAsync(ICollection<Employee> employees);
    Task<Employee> GetByIdAsync(Guid id);
}
