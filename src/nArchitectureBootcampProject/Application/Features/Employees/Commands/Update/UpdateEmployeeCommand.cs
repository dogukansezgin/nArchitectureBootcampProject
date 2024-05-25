using Application.Features.Employees.Constants;
using Application.Features.Employees.Rules;
using Application.Services.Employees;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Employees.Constants.EmployeesOperationClaims;

namespace Application.Features.Employees.Commands.Update;

public class UpdateEmployeeCommand
    : IRequest<UpdatedEmployeeResponse>
    //,
    //    ISecuredRequest,
    //    ICacheRemoverRequest,
    //    ILoggableRequest,
    //    ITransactionalRequest
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalIdentity { get; set; }
    public string Position { get; set; }

    public string[] Roles => [Admin, Write, EmployeesOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetEmployees"];

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, UpdatedEmployeeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public UpdateEmployeeCommandHandler(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<UpdatedEmployeeResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            Employee? employee = await _employeeService.GetAsync(
                predicate: e => e.Id == request.Id,
                cancellationToken: cancellationToken
            );

            employee = _mapper.Map(request, employee);
            employee.UserName = $"{request.FirstName} {request.LastName}";

            employee = await _employeeService.UpdateAsync(employee!);

            UpdatedEmployeeResponse response = _mapper.Map<UpdatedEmployeeResponse>(employee);
            return response;
        }
    }
}
