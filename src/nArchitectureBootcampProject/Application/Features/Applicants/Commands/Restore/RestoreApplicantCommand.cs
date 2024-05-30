using Application.Features.Applicants.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.Applicants;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Applicants.Constants.ApplicantsOperationClaims;

namespace Application.Features.Applicants.Commands.Restore;

public class RestoreApplicantCommand : IRequest<RestoredApplicantResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplicants"];

    public class RestoreApplicantCommandHandler : IRequestHandler<RestoreApplicantCommand, RestoredApplicantResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicantService _applicantService;

        public RestoreApplicantCommandHandler(IMapper mapper, IApplicantService applicantService)
        {
            _mapper = mapper;
            _applicantService = applicantService;
        }

        public async Task<RestoredApplicantResponse> Handle(RestoreApplicantCommand request, CancellationToken cancellationToken)
        {
            Applicant? applicant = await _applicantService.GetAsync(
                predicate: b => b.Id == request.Id && b.DeletedDate != null,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _applicantService.RestoreAsync(applicant!);

            RestoredApplicantResponse response = _mapper.Map<RestoredApplicantResponse>(applicant);
            return response;
        }
    }
}
