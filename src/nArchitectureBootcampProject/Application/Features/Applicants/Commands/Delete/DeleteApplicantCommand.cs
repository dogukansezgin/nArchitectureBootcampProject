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

namespace Application.Features.Applicants.Commands.Delete;

public class DeleteApplicantCommand : IRequest<DeletedApplicantResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplicants"];

    public class DeleteApplicantCommandHandler : IRequestHandler<DeleteApplicantCommand, DeletedApplicantResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicantService _applicantService;

        public DeleteApplicantCommandHandler(IMapper mapper, IApplicantService applicantService)
        {
            _mapper = mapper;
            _applicantService = applicantService;
        }

        public async Task<DeletedApplicantResponse> Handle(DeleteApplicantCommand request, CancellationToken cancellationToken)
        {
            Applicant? applicant = await _applicantService.GetAsync(
                predicate: a => a.Id == request.Id,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            applicant = await _applicantService.DeleteAsync(applicant!, request.IsPermament);

            DeletedApplicantResponse response = _mapper.Map<DeletedApplicantResponse>(applicant);
            response.IsPermament = request.IsPermament;
            response.DeletedDate = request.IsPermament ? DateTime.UtcNow : response.DeletedDate;
            return response;
        }
    }
}
