using Application.Features.Applicants.Constants;
using Application.Features.Employees.Constants;
using Application.Services.Employees;
using Application.Services.OperationClaims;
using Application.Services.UserOperationClaims;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Security.Hashing;
using static Application.Features.Employees.Constants.EmployeesOperationClaims;

namespace Application.Features.Employees.Commands.Create;

public class CreateEmployeeCommand : IRequest<CreatedEmployeeResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalIdentity { get; set; }
    public string Position { get; set; }

    public string[] Roles => [Admin, Write, EmployeesOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetEmployees"];

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, CreatedEmployeeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IOperationClaimService _operationClaimService;

        public CreateEmployeeCommandHandler(
            IMapper mapper,
            IEmployeeService employeeService,
            IUserOperationClaimService userOperationClaimService,
            IOperationClaimService operationClaimService
        )
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _userOperationClaimService = userOperationClaimService;
            _operationClaimService = operationClaimService;
        }

        public async Task<CreatedEmployeeResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            Employee employee = _mapper.Map<Employee>(request);
            employee.UserName = $"{request.FirstName} {request.LastName}";

            HashingHelper.CreatePasswordHash(
                request.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );
            employee.PasswordHash = passwordHash;
            employee.PasswordSalt = passwordSalt;

            employee = await _employeeService.AddAsync(employee);

            ICollection<OperationClaim> operationClaims = [];
            ICollection<UserOperationClaim> userOperationClaims = [];

            foreach (var item in EmployeesOperationClaims.InitialRoles)
            {
                var operationClaim = await _operationClaimService.GetListAsync(x => x.Name.Contains(item));
                if (operationClaim != null)
                    operationClaims.Add(operationClaim.Items.First());
            }

            if (operationClaims != null)
            {
                foreach (var item in operationClaims)
                {
                    userOperationClaims.Add(new UserOperationClaim() { UserId = employee.Id, OperationClaimId = item.Id });
                }
                userOperationClaims = await _userOperationClaimService.AddRangeAsync(userOperationClaims);
            }

            CreatedEmployeeResponse response = _mapper.Map<CreatedEmployeeResponse>(employee);
            return response;
        }
    }
}
