using System.Linq.Expressions;
using Application.Features.Employees.Rules;
using Application.Features.Employees.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Services.Employees;

public class EmployeeManager : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly EmployeeBusinessRules _employeeBusinessRules;

    public EmployeeManager(IEmployeeRepository employeeRepository, EmployeeBusinessRules employeeBusinessRules)
    {
        _employeeRepository = employeeRepository;
        _employeeBusinessRules = employeeBusinessRules;
    }

    public async Task<Employee?> GetAsync(
        Expression<Func<Employee, bool>> predicate,
        Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Employee? employee = await _employeeRepository.GetAsync(
            predicate,
            include,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return employee;
    }

    public async Task<IPaginate<Employee>?> GetListAsync(
        Expression<Func<Employee, bool>>? predicate = null,
        Func<IQueryable<Employee>, IOrderedQueryable<Employee>>? orderBy = null,
        Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Employee> employeeList = await _employeeRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return employeeList;
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        await _employeeBusinessRules.EmployeeShouldNotExist(employee);

        Employee addedEmployee = await _employeeRepository.AddAsync(employee);

        return addedEmployee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        await _employeeBusinessRules.EmployeeShouldExistWhenSelected(employee);
        await _employeeBusinessRules.EmployeeIdShouldExistWhenSelected(employee.Id);
        await _employeeBusinessRules.EmployeeShouldNotExistWhenUpdate(employee);

        Employee updatedEmployee = await _employeeRepository.UpdateAsync(employee);

        return updatedEmployee;
    }

    public async Task<Employee> DeleteAsync(Employee employee, bool permanent = false)
    {
        await _employeeBusinessRules.EmployeeShouldExistWhenSelected(employee);

        Employee deletedEmployee = await _employeeRepository.DeleteAsync(employee, permanent);

        return deletedEmployee;
    }

    public async Task<ICollection<Employee>> DeleteRangeAsync(ICollection<Employee> employees, bool permanent = false)
    {
        foreach (Employee employee in employees)
        {
            await _employeeBusinessRules.EmployeeShouldExistWhenSelected(employee);
        }

        ICollection<Employee> deletedEmployees = await _employeeRepository.DeleteRangeCustomAsync(employees, permanent);

        return deletedEmployees;
    }

    public async Task<Employee> RestoreAsync(Employee employee)
    {
        await _employeeBusinessRules.EmployeeShouldExistWhenSelected(employee);

        Employee restoredEmployee = await _employeeRepository.RestoreAsync(employee);

        return restoredEmployee;
    }

    public async Task<ICollection<Employee>> RestoreRangeAsync(ICollection<Employee> employees)
    {
        foreach (Employee employee in employees)
        {
            await _employeeBusinessRules.EmployeeShouldExistWhenSelected(employee);
        }

        ICollection<Employee> deletedEmployees = await _employeeRepository.RestoreRangeCustomAsync(employees);

        return deletedEmployees;
    }

    public async Task<Employee> GetByIdAsync(Guid id)
    {
        Employee? employee = await GetAsync(x => x.Id == id);

        await _employeeBusinessRules.EmployeeShouldExistWhenSelected(employee);

        return employee;
    }
}
