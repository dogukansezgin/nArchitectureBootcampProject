using Application.Features.Employees.Constants;
using Application.Features.Employees.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Employees.Rules;

public class EmployeeBusinessRules : BaseBusinessRules
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILocalizationService _localizationService;

    public EmployeeBusinessRules(IEmployeeRepository employeeRepository, ILocalizationService localizationService)
    {
        _employeeRepository = employeeRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, EmployeesBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task EmployeeShouldExistWhenSelected(Employee? employee)
    {
        if (employee == null)
            await throwBusinessException(EmployeesBusinessMessages.EmployeeNotExists);
    }

    public async Task EmployeeIdShouldExistWhenSelected(Guid id)
    {
        Employee? employee = await _employeeRepository.GetAsync(predicate: e => e.Id == id, enableTracking: false);
        await EmployeeShouldExistWhenSelected(employee);
    }

    public async Task EmployeeShouldNotExist(Employee employee)
    {
        var isExistId = await _employeeRepository.GetAsync(x => x.Id == employee.Id) is not null;
        var isExistUserName = await _employeeRepository.GetAsync(x => x.UserName.Trim() == employee.UserName.Trim()) is not null;

        bool isExistNationalId = false;
        if (employee.NationalIdentity != null)
        {
            isExistNationalId =
                await _employeeRepository.GetAsync(x => x.NationalIdentity.Trim() == employee.NationalIdentity.Trim())
                    is not null;
        }

        var isExistEmail = await _employeeRepository.GetAsync(x => x.Email.Trim() == employee.Email.Trim()) is not null;

        if (isExistId || isExistUserName || isExistNationalId || isExistEmail)
            await throwBusinessException(EmployeesBusinessMessages.EmployeeExists);
    }

    public async Task EmployeeShouldNotExistWhenUpdate(Employee employee)
    {
        bool isExistUserName = false;
        bool isExistNationalId = false;
        bool isExistEmail = false;

        if (employee.UserName is not null)
            isExistUserName =
                await _employeeRepository.GetAsync(x => x.Id != employee.Id && x.UserName.Trim() == employee.UserName.Trim())
                    is not null;

        if (employee.NationalIdentity is not null)
            isExistNationalId =
                await _employeeRepository.GetAsync(x =>
                    x.Id != employee.Id && x.NationalIdentity.Trim() == employee.NationalIdentity.Trim()
                )
                    is not null;

        if (employee.Email is not null)
            isExistEmail =
                await _employeeRepository.GetAsync(x => x.Id != employee.Id && x.Email.Trim() == employee.Email.Trim())
                    is not null;

        if (isExistUserName || isExistNationalId || isExistEmail)
            await throwBusinessException(EmployeesBusinessMessages.EmployeeExists);
    }
}
