using Application.Features.Applicants.Constants;
using Application.Features.Instructors.Constants;
using Application.Services.Applicants;
using Application.Services.OperationClaims;
using Application.Services.UserOperationClaims;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Security.Entities;
using NArchitecture.Core.Security.Hashing;
using static Application.Features.Applicants.Constants.ApplicantsOperationClaims;

namespace Application.Features.Applicants.Commands.Create;

public class CreateApplicantCommand
    : IRequest<CreatedApplicantResponse>
    //,
    //    ISecuredRequest,
    //    ICacheRemoverRequest,
    //    ILoggableRequest,
    //    ITransactionalRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalIdentity { get; set; }
    public string About { get; set; }

    public string[] Roles => [Admin, Write, ApplicantsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplicants"];

    public class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, CreatedApplicantResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicantService _applicantService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IOperationClaimService _operationClaimService;

        public CreateApplicantCommandHandler(IMapper mapper, IApplicantService applicantService, IUserOperationClaimService userOperationClaimService, IOperationClaimService operationClaimService)
        {
            _mapper = mapper;
            _applicantService = applicantService;
            _userOperationClaimService = userOperationClaimService;
            _operationClaimService = operationClaimService;
        }

        public async Task<CreatedApplicantResponse> Handle(CreateApplicantCommand request, CancellationToken cancellationToken)
        {
            Applicant applicant = _mapper.Map<Applicant>(request);
            applicant.UserName = $"{request.FirstName} {request.LastName}";

            HashingHelper.CreatePasswordHash(
                request.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );
            applicant.PasswordHash = passwordHash;
            applicant.PasswordSalt = passwordSalt;

            applicant = await _applicantService.AddAsync(applicant);


            ICollection<OperationClaim> operationClaims = [];
            ICollection<UserOperationClaim> userOperationClaims = [];

            foreach (var item in ApplicantsOperationClaims.InitialRoles)
            {
                var operationClaim = await _operationClaimService.GetListAsync(x => x.Name.Contains(item));
                if (operationClaim != null)
                    operationClaims.Add(operationClaim.Items.First());
            }

            if (operationClaims != null)
            {
                foreach (var item in operationClaims)
                {
                    userOperationClaims.Add(
                        new UserOperationClaim() { UserId = applicant.Id, OperationClaimId = item.Id }
                    );
                }
                userOperationClaims = await _userOperationClaimService.AddRangeAsync(userOperationClaims);
            }


            CreatedApplicantResponse response = _mapper.Map<CreatedApplicantResponse>(applicant);
            return response;
        }
    }
}
