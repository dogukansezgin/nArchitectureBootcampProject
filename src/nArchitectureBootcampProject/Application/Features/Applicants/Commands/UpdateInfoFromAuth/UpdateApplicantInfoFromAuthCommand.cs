using Application.Features.Applicants.Constants;
using Application.Features.Applicants.Rules;
using Application.Services.Applicants;
using Application.Services.AuthService;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Applicants.Constants.ApplicantsOperationClaims;


namespace Application.Features.Applicants.Commands.UpdateInfoFromAuth;
public class UpdateApplicantInfoFromAuthCommand : IRequest<UpdatedApplicantInfoFromAuthResponse>, ISecuredRequest,
        ICacheRemoverRequest,
        ILoggableRequest,
        ITransactionalRequest
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalIdentity { get; set; }
    public string? About { get; set; }

    public string[] Roles => [Admin, ApplicantsOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplicants"];

    public UpdateApplicantInfoFromAuthCommand()
    {
       
    }

    public UpdateApplicantInfoFromAuthCommand(Guid id, string email, string userName, string? firstName, string? lastName, DateTime? dateOfBirth, string? nationalIdentity, string about)
    {
        Id = id;
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        NationalIdentity = nationalIdentity;
        About = about;
    }

    public class UpdateApplicantInfoFromAuthCommandHandler : IRequestHandler<UpdateApplicantInfoFromAuthCommand, UpdatedApplicantInfoFromAuthResponse>
    {
        private readonly IApplicantService _applicantService;
        private readonly IMapper _mapper;
        private readonly ApplicantBusinessRules _applicantBusinessRules;
        private readonly IAuthService _authService;

        public UpdateApplicantInfoFromAuthCommandHandler(IApplicantService applicantService, IMapper mapper, ApplicantBusinessRules applicantBusinessRules, IAuthService authService)
        {
            _applicantService = applicantService;
            _mapper = mapper;
            _applicantBusinessRules = applicantBusinessRules;
            _authService = authService;
        }

        public async Task<UpdatedApplicantInfoFromAuthResponse> Handle(UpdateApplicantInfoFromAuthCommand request, CancellationToken cancellationToken)
        {
            Applicant? applicant = await _applicantService.GetAsync(predicate: x => x.Id == request.Id);

            await _applicantBusinessRules.ApplicantShouldExistWhenSelected(applicant);
            await _applicantBusinessRules.ApplicantShouldNotExistUpdate(applicant!);
             
            applicant = _mapper.Map(request, applicant);

            Applicant updatedApplicant = await _applicantService.UpdateAsync(applicant!);

            UpdatedApplicantInfoFromAuthResponse response = _mapper.Map<UpdatedApplicantInfoFromAuthResponse>(updatedApplicant);
            response.AccessToken = await _authService.CreateAccessToken(applicant!);
            return response;
        }
    }
}
