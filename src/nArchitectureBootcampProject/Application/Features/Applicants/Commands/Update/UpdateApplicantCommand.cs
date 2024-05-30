using Application.Features.Applicants.Constants;
using Application.Features.Applicants.Rules;
using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.Applicants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Applicants.Constants.ApplicantsOperationClaims;

namespace Application.Features.Applicants.Commands.Update;

public class UpdateApplicantCommand : IRequest<UpdatedApplicantResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalIdentity { get; set; }
    public string? About { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplicants"];

    public class UpdateApplicantCommandHandler : IRequestHandler<UpdateApplicantCommand, UpdatedApplicantResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicantService _applicantService;

        public UpdateApplicantCommandHandler(IMapper mapper, IApplicantService applicantService)
        {
            _mapper = mapper;
            _applicantService = applicantService;
        }

        public async Task<UpdatedApplicantResponse> Handle(UpdateApplicantCommand request, CancellationToken cancellationToken)
        {
            Applicant? applicant = await _applicantService.GetAsync(
                predicate: a => a.Id == request.Id,
                cancellationToken: cancellationToken
            );

            applicant = _mapper.Map(request, applicant);
            applicant.UserName = $"{request.FirstName} {request.LastName}";

            applicant = await _applicantService.UpdateAsync(applicant!);

            UpdatedApplicantResponse response = _mapper.Map<UpdatedApplicantResponse>(applicant);
            return response;
        }
    }
}
