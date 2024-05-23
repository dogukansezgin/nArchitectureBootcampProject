using Application.Features.Applicants.Constants;
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

namespace Application.Features.Applicants.Commands.RestoreRange;

public class RestoreRangeApplicantCommand : IRequest<RestoredRangeApplicantResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }

    public string[] Roles => [Admin, Write];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetApplicants" };

    public class RestoreRangeApplicantCommandHandler : IRequestHandler<RestoreRangeApplicantCommand, RestoredRangeApplicantResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicantService _applicantService;

        public RestoreRangeApplicantCommandHandler(IMapper mapper, IApplicantService applicantService)
        {
            _mapper = mapper;
            _applicantService = applicantService;
        }

        public async Task<RestoredRangeApplicantResponse> Handle(
            RestoreRangeApplicantCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Applicant>? applicants = await _applicantService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );
            Console.WriteLine(applicants.Items);
            await _applicantService.RestoreRangeAsync(applicants.Items);

            RestoredRangeApplicantResponse response = new RestoredRangeApplicantResponse
            {
                Ids = applicants.Items.Select(b => b.Id).ToList()
            };

            return response;
        }
    }
}
