using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.Applicants;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Applicants.Constants.ApplicantsOperationClaims;

namespace Application.Features.Applicants.Queries.GetListDeleted;

public class GetListDeletedApplicantQuery : IRequest<GetListResponse<GetListDeletedApplicantListItemDto>>, ISecuredRequest/*, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListApplicants({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetApplicants";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListApplicantQueryHandler
        : IRequestHandler<GetListDeletedApplicantQuery, GetListResponse<GetListDeletedApplicantListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicantService _applicantService;

        public GetListApplicantQueryHandler(IMapper mapper, IApplicantService applicantService)
        {
            _mapper = mapper;
            _applicantService = applicantService;
        }

        public async Task<GetListResponse<GetListDeletedApplicantListItemDto>> Handle(
            GetListDeletedApplicantQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Applicant>? applicants = await _applicantService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                withDeleted: true,
                predicate: x => x.DeletedDate != null
            );

            GetListResponse<GetListDeletedApplicantListItemDto> response = _mapper.Map<
                GetListResponse<GetListDeletedApplicantListItemDto>
            >(applicants);
            return response;
        }
    }
}
