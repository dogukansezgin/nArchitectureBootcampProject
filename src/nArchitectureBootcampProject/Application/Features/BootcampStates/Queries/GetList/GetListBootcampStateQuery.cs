using Application.Features.Applicants.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.BootcampStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.BootcampStates.Constants.BootcampStatesOperationClaims;

namespace Application.Features.BootcampStates.Queries.GetList;

public class GetListBootcampStateQuery : IRequest<GetListResponse<GetListBootcampStateListItemDto>>, ISecuredRequest /*, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles =>
        [
            UsersOperationClaims.Admin,
            EmployeesOperationClaims.User,
            InstructorsOperationClaims.User,
            ApplicantsOperationClaims.User
        ];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListBootcampStates({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetBootcampStates";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListBootcampStateQueryHandler
        : IRequestHandler<GetListBootcampStateQuery, GetListResponse<GetListBootcampStateListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampStateService _bootcampStateService;

        public GetListBootcampStateQueryHandler(IMapper mapper, IBootcampStateService bootcampStateService)
        {
            _mapper = mapper;
            _bootcampStateService = bootcampStateService;
        }

        public async Task<GetListResponse<GetListBootcampStateListItemDto>> Handle(
            GetListBootcampStateQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<BootcampState>? bootcampStates = await _bootcampStateService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListBootcampStateListItemDto> response = _mapper.Map<
                GetListResponse<GetListBootcampStateListItemDto>
            >(bootcampStates);
            return response;
        }
    }
}
