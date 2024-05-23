using Application.Features.Applicants.Constants;
using Application.Features.Instructors.Constants;
using Application.Services.Instructors;
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
using static Application.Features.Instructors.Constants.InstructorsOperationClaims;

namespace Application.Features.Instructors.Commands.Create;

public class CreateInstructorCommand : IRequest<CreatedInstructorResponse>
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
    public string? CompanyName { get; set; }

    public string[] Roles => [Admin, Write, InstructorsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetInstructors"];

    public class CreateInstructorCommandHandler : IRequestHandler<CreateInstructorCommand, CreatedInstructorResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorService _instructorService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IOperationClaimService _operationClaimService;

        public CreateInstructorCommandHandler(
            IMapper mapper,
            IInstructorService instructorService,
            IUserOperationClaimService userOperationClaimService,
            IOperationClaimService operationClaimService
        )
        {
            _mapper = mapper;
            _instructorService = instructorService;
            _userOperationClaimService = userOperationClaimService;
            _operationClaimService = operationClaimService;
        }

        public async Task<CreatedInstructorResponse> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
        {
            Instructor instructor = _mapper.Map<Instructor>(request);
            instructor.UserName = $"{request.FirstName} {request.LastName}";

            HashingHelper.CreatePasswordHash(
                request.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );
            instructor.PasswordHash = passwordHash;
            instructor.PasswordSalt = passwordSalt;

            instructor = await _instructorService.AddAsync(instructor);

            ICollection<OperationClaim> operationClaims = [];
            ICollection<UserOperationClaim> userOperationClaims = [];

            foreach (var item in InstructorsOperationClaims.InitialRoles)
            {
                var operationClaim = await _operationClaimService.GetListAsync(x => x.Name.Contains(item));
                if (operationClaim != null)
                    operationClaims.Add(operationClaim.Items.First());
            }

            if (operationClaims != null)
            {
                foreach (var item in operationClaims)
                {
                    userOperationClaims.Add(new UserOperationClaim() { UserId = instructor.Id, OperationClaimId = item.Id });
                }
                userOperationClaims = await _userOperationClaimService.AddRangeAsync(userOperationClaims);
            }

            CreatedInstructorResponse response = _mapper.Map<CreatedInstructorResponse>(instructor);
            return response;
        }
    }
}
