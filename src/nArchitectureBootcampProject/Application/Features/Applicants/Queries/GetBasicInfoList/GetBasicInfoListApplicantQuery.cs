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

namespace Application.Features.Applicants.Queries.GetBasicInfoList;

public class GetBasicInfoListApplicantQuery : IRequest<GetListResponse<GetBasicInfoApplicantListItemDto>> /*, ISecuredRequest, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListApplicants({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetApplicants";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListApplicantQueryHandler
        : IRequestHandler<GetBasicInfoListApplicantQuery, GetListResponse<GetBasicInfoApplicantListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicantService _applicantService;

        public GetListApplicantQueryHandler(IMapper mapper, IApplicantService applicantService)
        {
            _mapper = mapper;
            _applicantService = applicantService;
        }

        public async Task<GetListResponse<GetBasicInfoApplicantListItemDto>> Handle(
            GetBasicInfoListApplicantQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Applicant>? applicants = await _applicantService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetBasicInfoApplicantListItemDto> response = _mapper.Map<
                GetListResponse<GetBasicInfoApplicantListItemDto>
            >(applicants);
            return response;
        }
    }
}
