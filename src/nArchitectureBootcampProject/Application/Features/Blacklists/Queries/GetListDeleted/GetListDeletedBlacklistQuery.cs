using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.Blacklists;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Blacklists.Constants.BlacklistsOperationClaims;

namespace Application.Features.Blacklists.Queries.GetListDeleted;

public class GetListDeletedBlacklistQuery : IRequest<GetListResponse<GetListDeletedBlacklistListItemDto>>, ISecuredRequest /*, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListBlacklists({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetBlacklists";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListBlacklistQueryHandler
        : IRequestHandler<GetListDeletedBlacklistQuery, GetListResponse<GetListDeletedBlacklistListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBlacklistService _blacklistService;

        public GetListBlacklistQueryHandler(IMapper mapper, IBlacklistService blacklistService)
        {
            _mapper = mapper;
            _blacklistService = blacklistService;
        }

        public async Task<GetListResponse<GetListDeletedBlacklistListItemDto>> Handle(
            GetListDeletedBlacklistQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Blacklist>? blacklists = await _blacklistService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                include: x => x.Include(x => x.Applicant),
                withDeleted: true,
                predicate: x => x.DeletedDate != null
            );

            GetListResponse<GetListDeletedBlacklistListItemDto> response = _mapper.Map<
                GetListResponse<GetListDeletedBlacklistListItemDto>
            >(blacklists);
            return response;
        }
    }
}
