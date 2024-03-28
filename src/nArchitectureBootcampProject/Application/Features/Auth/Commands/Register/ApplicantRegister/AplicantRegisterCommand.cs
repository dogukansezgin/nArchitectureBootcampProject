﻿using Application.Features.Auth.Rules;
using Application.Services.Applicants;
using Application.Services.AuthService;
using Application.Services.OperationClaims;
using Application.Services.UserOperationClaims;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Security.Hashing;
using NArchitecture.Core.Security.JWT;

namespace Application.Features.Auth.Commands.Register.ApplicantRegister;
public class ApplicantRegisterCommand : IRequest<ApplicantRegisteredResponse>
{
    public ApplicantForRegisterDto ApplicantForRegisterDto { get; set; }
    public string IpAddress { get; set; }

    public ApplicantRegisterCommand()
    {
        ApplicantForRegisterDto = null!;
        IpAddress = string.Empty;
    }
    public ApplicantRegisterCommand(ApplicantForRegisterDto applicantForRegisterDto, string ipAddress)
    {
        ApplicantForRegisterDto = applicantForRegisterDto;
        IpAddress = ipAddress;
    }

    public class ApplicantRegisterCommandHandler : IRequestHandler<ApplicantRegisterCommand, ApplicantRegisteredResponse>
    {
        private readonly IApplicantService _applicantService;
        private readonly IAuthService _authService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly AuthBusinessRules _authBusinessRules;

        public ApplicantRegisterCommandHandler(IApplicantService applicantService, IAuthService authService, AuthBusinessRules authBusinessRules, IUserOperationClaimService userOperationClaimService, IOperationClaimService operationClaimService)
        {
            _applicantService = applicantService;
            _authService = authService;
            _authBusinessRules = authBusinessRules;
            _userOperationClaimService = userOperationClaimService;
            _operationClaimService = operationClaimService;
        }

        public async Task<ApplicantRegisteredResponse> Handle(ApplicantRegisterCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.UserEmailShouldBeNotExists(request.ApplicantForRegisterDto.Email);

            HashingHelper.CreatePasswordHash(
                request.ApplicantForRegisterDto.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
                );

            Applicant newApplicant =
                new()
                {
                    UserName = request.ApplicantForRegisterDto.UserName,
                    FirstName = request.ApplicantForRegisterDto.FirstName,
                    LastName = request.ApplicantForRegisterDto.LastName,
                    DateOfBirth = request.ApplicantForRegisterDto.DateOfBirth,
                    NationalIdentity = request.ApplicantForRegisterDto.NationalIdentity,
                    About = request.ApplicantForRegisterDto.About,
                    Email = request.ApplicantForRegisterDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };
            Applicant createdApplicant = await _applicantService.AddAsync( newApplicant );

            ICollection<UserOperationClaim> userOperationClaims = [];

            var operationClaims = await _operationClaimService.GetListAsync(x => x.Name.Contains("Applicants"));
            foreach (var item in operationClaims.Items)
            {
                userOperationClaims.Add(new UserOperationClaim() { UserId = createdApplicant.Id, OperationClaimId = item.Id } );
            }

            userOperationClaims = await _userOperationClaimService.AddRangeAsync( userOperationClaims );

            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdApplicant);

            Domain.Entities.RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(
                createdApplicant,
                request.IpAddress
                );
            Domain.Entities.RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            ApplicantRegisteredResponse registeredResponse = new() { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };
            return registeredResponse;
        }
    }
}