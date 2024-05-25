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

namespace Application.Features.Employees.Commands.DeleteRange;

public class DeleteRangeEmployeeCommand : IRequest<DeletedRangeEmployeeResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [Admin, Write, EmployeesOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetEmployees" };

    public class DeleteRangeEmployeeCommandHandler : IRequestHandler<DeleteRangeEmployeeCommand, DeletedRangeEmployeeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public DeleteRangeEmployeeCommandHandler(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<DeletedRangeEmployeeResponse> Handle(
            DeleteRangeEmployeeCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Employee>? employees = await _employeeService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _employeeService.DeleteRangeAsync(employees.Items, request.IsPermament);

            DeletedRangeEmployeeResponse response = new DeletedRangeEmployeeResponse
            {
                Ids = employees.Items.Select(b => b.Id).ToList(),
                DeletedDate = DateTime.UtcNow,
                IsPermament = request.IsPermament
            };

            return response;
        }
    }
}
