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

    public async Task EmployeeShouldExist(Guid id)
    {
        var isExist = _employeeRepository.Get(x => x.Id == id) is null;
        if (isExist)
            await throwBusinessException(EmployeesBusinessMessages.EmployeeNotExists);
    }

    public async Task EmployeeShouldNotExist(Employee employee)
    {
        var isExistId = await _employeeRepository.GetAsync(x => x.Id == employee.Id) is not null;
        var isExistUserName = await _employeeRepository.GetAsync(x => x.UserName.Trim() == employee.UserName.Trim()) is not null;
        var isExistNationalId =
            await _employeeRepository.GetAsync(x => x.NationalIdentity.Trim() == employee.NationalIdentity.Trim()) is not null;
        var isExistEmail = await _employeeRepository.GetAsync(x => x.Email.Trim() == employee.Email.Trim()) is not null;

        if (isExistId || isExistUserName || isExistNationalId || isExistEmail)
            await throwBusinessException(EmployeesBusinessMessages.EmployeeExists);
    }
}
