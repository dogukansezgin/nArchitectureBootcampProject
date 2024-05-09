using Application.Features.Auth.Commands.Register.InstructorRegister;
using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Instructors;
using Application.Services.OperationClaims;
using Application.Services.UserOperationClaims;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Security.Hashing;
using NArchitecture.Core.Security.JWT;

namespace Application.Features.Auth.Commands.Register.InstructorRegister;

public class InstructorRegisterCommand : IRequest<InstructorRegisteredResponse>
{
    public InstructorForRegisterDto InstructorForRegisterDto { get; set; }
    public string IpAddress { get; set; }

    public InstructorRegisterCommand()
    {
        InstructorForRegisterDto = null!;
        IpAddress = string.Empty;
    }

    public InstructorRegisterCommand(InstructorForRegisterDto instructorForRegisterDto, string ipAddress)
    {
        InstructorForRegisterDto = instructorForRegisterDto;
        IpAddress = ipAddress;
    }

    public class InstructorRegisterCommandHandler : IRequestHandler<InstructorRegisterCommand, InstructorRegisteredResponse>
    {
        private readonly IInstructorService _instructorService;
        private readonly IAuthService _authService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly AuthBusinessRules _authBusinessRules;

        public InstructorRegisterCommandHandler(
            IInstructorService instructorService,
            IAuthService authService,
            AuthBusinessRules authBusinessRules,
            IUserOperationClaimService userOperationClaimService,
            IOperationClaimService operationClaimService
        )
        {
            _instructorService = instructorService;
            _authService = authService;
            _authBusinessRules = authBusinessRules;
            _userOperationClaimService = userOperationClaimService;
            _operationClaimService = operationClaimService;
        }

        public async Task<InstructorRegisteredResponse> Handle(
            InstructorRegisterCommand request,
            CancellationToken cancellationToken
        )
        {
            await _authBusinessRules.UserEmailShouldBeNotExists(request.InstructorForRegisterDto.Email);

            HashingHelper.CreatePasswordHash(
                request.InstructorForRegisterDto.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );

            Instructor newInstructor =
                new()
                {
                    UserName =
                        request.InstructorForRegisterDto.FirstName.Trim()
                        + " "
                        + request.InstructorForRegisterDto.LastName.Trim(),
                    FirstName = request.InstructorForRegisterDto.FirstName,
                    LastName = request.InstructorForRegisterDto.LastName,
                    CompanyName = request.InstructorForRegisterDto.CompanyName,
                    Email = request.InstructorForRegisterDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };
            Instructor createdInstructor = await _instructorService.AddAsync(newInstructor);

            ICollection<UserOperationClaim> userOperationClaims = [];

            var operationClaims = await _operationClaimService.GetListAsync(x => x.Name.Contains("Instructors"));
            foreach (var item in operationClaims.Items)
            {
                userOperationClaims.Add(new UserOperationClaim() { UserId = createdInstructor.Id, OperationClaimId = item.Id });
            }

            userOperationClaims = await _userOperationClaimService.AddRangeAsync(userOperationClaims);

            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdInstructor);

            Domain.Entities.RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(
                createdInstructor,
                request.IpAddress
            );
            Domain.Entities.RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            InstructorRegisteredResponse registeredResponse =
                new() { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };
            return registeredResponse;
        }
    }
}
