using Application.Features.Employees.Constants;
using Application.Services.Employees;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Employees.Constants.EmployeesOperationClaims;

namespace Application.Features.Employees.Commands.RestoreRange;

public class RestoreRangeEmployeeCommand : IRequest<RestoredRangeEmployeeResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }

    public string[] Roles => [Admin, Write];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetEmployees" };

    public class RestoreRangeEmployeeCommandHandler : IRequestHandler<RestoreRangeEmployeeCommand, RestoredRangeEmployeeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public RestoreRangeEmployeeCommandHandler(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<RestoredRangeEmployeeResponse> Handle(
            RestoreRangeEmployeeCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Employee>? employees = await _employeeService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _employeeService.RestoreRangeAsync(employees.Items);

            RestoredRangeEmployeeResponse response = new RestoredRangeEmployeeResponse
            {
                Ids = employees.Items.Select(b => b.Id).ToList()
            };

            return response;
        }
    }
}
