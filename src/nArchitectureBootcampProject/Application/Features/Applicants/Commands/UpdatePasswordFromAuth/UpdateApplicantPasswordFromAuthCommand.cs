using Application.Features.Applicants.Constants;
using Application.Features.Applicants.Rules;
using Application.Features.Auth.Rules;
using Application.Features.Employees.Constants;
using Application.Features.UserOperationClaims.Constants;
using Application.Features.Users.Constants;
using Application.Services.Applicants;
using Application.Services.AuthService;
using Application.Services.UsersService;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Security.Hashing;
using static Application.Features.Applicants.Constants.ApplicantsOperationClaims;

namespace Application.Features.Applicants.Commands.UpdatePasswordFromAuth;

public class UpdateApplicantPasswordFromAuthCommand : IRequest<UpdatedApplicantPasswordFromAuthResponse>, ISecuredRequest
//ICacheRemoverRequest,
//ILoggableRequest,
//ITransactionalRequest
{
    public Guid Id { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, ApplicantsOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplicants"];

    public UpdateApplicantPasswordFromAuthCommand() { }

    public UpdateApplicantPasswordFromAuthCommand(Guid id, string currentPassword, string newPassword)
    {
        Id = id;
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }

    public class UpdateApplicantPasswordFromAuthCommandHandler
        : IRequestHandler<UpdateApplicantPasswordFromAuthCommand, UpdatedApplicantPasswordFromAuthResponse>
    {
        private readonly IApplicantService _applicantService;
        private readonly IMapper _mapper;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthService _authService;

        public UpdateApplicantPasswordFromAuthCommandHandler(
            IApplicantService applicantService,
            IMapper mapper,
            AuthBusinessRules authBusinessRules,
            IAuthService authService
        )
        {
            _applicantService = applicantService;
            _mapper = mapper;
            _authBusinessRules = authBusinessRules;
            _authService = authService;
        }

        public async Task<UpdatedApplicantPasswordFromAuthResponse> Handle(
            UpdateApplicantPasswordFromAuthCommand request,
            CancellationToken cancellationToken
        )
        {
            Applicant? applicant = await _applicantService.GetAsync(predicate: x => x.Id == request.Id);

            await _authBusinessRules.UserShouldBeExistsWhenSelected(applicant);
            await _authBusinessRules.UserPasswordShouldBeMatch(applicant!, request.CurrentPassword);

            HashingHelper.CreatePasswordHash(
                request.NewPassword,
                passwordHash: out byte[] _passwordHash,
                passwordSalt: out byte[] _passwordSalt
            );

            applicant = _mapper.Map(request, applicant);
            applicant.PasswordHash = _passwordHash;
            applicant.PasswordSalt = _passwordSalt;

            Applicant updatedApplicant = await _applicantService.UpdateAsync(applicant!);

            UpdatedApplicantPasswordFromAuthResponse response = _mapper.Map<UpdatedApplicantPasswordFromAuthResponse>(
                updatedApplicant
            );
            response.AccessToken = await _authService.CreateAccessToken(applicant!);
            return response;
        }
    }
}
