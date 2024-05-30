using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.ApplicationStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.ApplicationStates.Constants.ApplicationStatesOperationClaims;

namespace Application.Features.ApplicationStates.Queries.GetListDeleted;

public class GetListDeletedApplicationStateQuery : IRequest<GetListResponse<GetListDeletedApplicationStateListItemDto>>, ISecuredRequest/*, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListApplicationStates({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetApplicationStates";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListApplicationStateQueryHandler
        : IRequestHandler<GetListDeletedApplicationStateQuery, GetListResponse<GetListDeletedApplicationStateListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationStateService _applicationStateService;

        public GetListApplicationStateQueryHandler(IMapper mapper, IApplicationStateService applicationStateService)
        {
            _mapper = mapper;
            _applicationStateService = applicationStateService;
        }

        public async Task<GetListResponse<GetListDeletedApplicationStateListItemDto>> Handle(
            GetListDeletedApplicationStateQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<ApplicationState>? applicationStates = await _applicationStateService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                withDeleted: true,
                predicate: x => x.DeletedDate != null
            );

            GetListResponse<GetListDeletedApplicationStateListItemDto> response = _mapper.Map<
                GetListResponse<GetListDeletedApplicationStateListItemDto>
            >(applicationStates);
            return response;
        }
    }
}
