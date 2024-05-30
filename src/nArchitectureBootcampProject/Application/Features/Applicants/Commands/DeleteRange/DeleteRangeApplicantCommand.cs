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
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Applicants.Constants.ApplicantsOperationClaims;

namespace Application.Features.Applicants.Commands.DeleteRange;

public class DeleteRangeApplicantCommand : IRequest<DeletedRangeApplicantResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetApplicants" };

    public class DeleteRangeApplicantCommandHandler : IRequestHandler<DeleteRangeApplicantCommand, DeletedRangeApplicantResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicantService _applicantService;

        public DeleteRangeApplicantCommandHandler(IMapper mapper, IApplicantService applicantService)
        {
            _mapper = mapper;
            _applicantService = applicantService;
        }

        public async Task<DeletedRangeApplicantResponse> Handle(
            DeleteRangeApplicantCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Applicant>? applicants = await _applicantService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _applicantService.DeleteRangeAsync(applicants.Items, request.IsPermament);

            DeletedRangeApplicantResponse response = new DeletedRangeApplicantResponse
            {
                Ids = applicants.Items.Select(b => b.Id).ToList(),
                DeletedDate = DateTime.UtcNow,
                IsPermament = request.IsPermament
            };

            return response;
        }
    }
}
