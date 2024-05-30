using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.Employees;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Employees.Constants.EmployeesOperationClaims;

namespace Application.Features.Employees.Commands.Delete;

public class DeleteEmployeeCommand : IRequest<DeletedEmployeeResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetEmployees"];

    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, DeletedEmployeeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public DeleteEmployeeCommandHandler(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<DeletedEmployeeResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            Employee? employee = await _employeeService.GetAsync(
                predicate: e => e.Id == request.Id,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            employee = await _employeeService.DeleteAsync(employee!);

            DeletedEmployeeResponse response = _mapper.Map<DeletedEmployeeResponse>(employee);
            response.IsPermament = request.IsPermament;
            response.DeletedDate = request.IsPermament ? DateTime.UtcNow : response.DeletedDate;
            return response;
        }
    }
}
