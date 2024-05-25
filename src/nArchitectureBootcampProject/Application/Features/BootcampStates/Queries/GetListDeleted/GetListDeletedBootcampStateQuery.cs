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

namespace Application.Features.BootcampStates.Queries.GetListDeleted;

public class GetListDeletedBootcampStateQuery : IRequest<GetListResponse<GetListDeletedBootcampStateListItemDto>> /*, ISecuredRequest, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListBootcampStates({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetBootcampStates";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListBootcampStateQueryHandler
        : IRequestHandler<GetListDeletedBootcampStateQuery, GetListResponse<GetListDeletedBootcampStateListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampStateService _bootcampStateService;

        public GetListBootcampStateQueryHandler(IMapper mapper, IBootcampStateService bootcampStateService)
        {
            _mapper = mapper;
            _bootcampStateService = bootcampStateService;
        }

        public async Task<GetListResponse<GetListDeletedBootcampStateListItemDto>> Handle(
            GetListDeletedBootcampStateQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<BootcampState>? bootcampStates = await _bootcampStateService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                withDeleted: true,
                predicate: x => x.DeletedDate != null
            );

            GetListResponse<GetListDeletedBootcampStateListItemDto> response = _mapper.Map<
                GetListResponse<GetListDeletedBootcampStateListItemDto>
            >(bootcampStates);
            return response;
        }
    }
}
