using Application.Features.Employees.Constants;
using Application.Services.Employees;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Employees.Constants.EmployeesOperationClaims;

namespace Application.Features.Employees.Commands.Restore;

public class RestoreEmployeeCommand : IRequest<RestoredEmployeeResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Write];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetEmployees"];

    public class RestoreEmployeeCommandHandler : IRequestHandler<RestoreEmployeeCommand, RestoredEmployeeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public RestoreEmployeeCommandHandler(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<RestoredEmployeeResponse> Handle(RestoreEmployeeCommand request, CancellationToken cancellationToken)
        {
            Employee? employee = await _employeeService.GetAsync(
                predicate: b => b.Id == request.Id && b.DeletedDate != null,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _employeeService.RestoreAsync(employee!);

            RestoredEmployeeResponse response = _mapper.Map<RestoredEmployeeResponse>(employee);
            return response;
        }
    }
}
