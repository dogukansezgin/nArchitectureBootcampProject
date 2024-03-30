using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Employees;
using Application.Services.OperationClaims;
using Application.Services.UserOperationClaims;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Security.Hashing;
using NArchitecture.Core.Security.JWT;

namespace Application.Features.Auth.Commands.Register.EmployeeRegister;

public class EmployeeRegisterCommand : IRequest<EmployeeRegisteredResponse>
{
    public EmployeeForRegisterDto EmployeeForRegisterDto { get; set; }
    public string IpAddress { get; set; }

    public EmployeeRegisterCommand()
    {
        EmployeeForRegisterDto = null!;
        IpAddress = string.Empty;
    }

    public EmployeeRegisterCommand(EmployeeForRegisterDto employeeForRegisterDto, string ipAddress)
    {
        EmployeeForRegisterDto = employeeForRegisterDto;
        IpAddress = ipAddress;
    }

    public class EmployeeRegisterCommandHandler : IRequestHandler<EmployeeRegisterCommand, EmployeeRegisteredResponse>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAuthService _authService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly AuthBusinessRules _authBusinessRules;

        public EmployeeRegisterCommandHandler(
            IEmployeeService employeeService,
            IAuthService authService,
            AuthBusinessRules authBusinessRules,
            IUserOperationClaimService userOperationClaimService,
            IOperationClaimService operationClaimService
        )
        {
            _employeeService = employeeService;
            _authService = authService;
            _authBusinessRules = authBusinessRules;
            _userOperationClaimService = userOperationClaimService;
            _operationClaimService = operationClaimService;
        }

        public async Task<EmployeeRegisteredResponse> Handle(EmployeeRegisterCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.UserEmailShouldBeNotExists(request.EmployeeForRegisterDto.Email);

            HashingHelper.CreatePasswordHash(
                request.EmployeeForRegisterDto.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );

            Employee newEmployee =
                new()
                {
                    UserName = request.EmployeeForRegisterDto.UserName,
                    FirstName = request.EmployeeForRegisterDto.FirstName,
                    LastName = request.EmployeeForRegisterDto.LastName,
                    DateOfBirth = request.EmployeeForRegisterDto.DateOfBirth,
                    NationalIdentity = request.EmployeeForRegisterDto.NationalIdentity,
                    Position = request.EmployeeForRegisterDto.Position,
                    Email = request.EmployeeForRegisterDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };
            Employee createdEmployee = await _employeeService.AddAsync(newEmployee);

            ICollection<UserOperationClaim> userOperationClaims = [];

            var operationClaims = await _operationClaimService.GetListAsync(x => x.Name.Contains("Employees"));
            foreach (var item in operationClaims.Items)
            {
                userOperationClaims.Add(new UserOperationClaim() { UserId = createdEmployee.Id, OperationClaimId = item.Id });
            }

            userOperationClaims = await _userOperationClaimService.AddRangeAsync(userOperationClaims);

            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdEmployee);

            Domain.Entities.RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(
                createdEmployee,
                request.IpAddress
            );
            Domain.Entities.RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            EmployeeRegisteredResponse registeredResponse =
                new() { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };
            return registeredResponse;
        }
    }
}
