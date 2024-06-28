using Application.Features.Applicants.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.Bootcamps;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Bootcamps.Constants.BootcampsOperationClaims;

namespace Application.Features.Bootcamps.Queries.SearchAll;

public class SearchAllBootcampQuery : IRequest<GetListResponse<SearchAllBootcampListItemDto>> /*, ISecuredRequest, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [];

    public bool BypassCache { get; }
    public string? CacheKey => $"SearchAllBootcamps({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetBootcamps";
    public TimeSpan? SlidingExpiration { get; }

    public class SearchAllBootcampQueryHandler
        : IRequestHandler<SearchAllBootcampQuery, GetListResponse<SearchAllBootcampListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public SearchAllBootcampQueryHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<GetListResponse<SearchAllBootcampListItemDto>> Handle(
            SearchAllBootcampQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Bootcamp>? bootcamps = await _bootcampService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                orderBy: x => x.OrderByDescending(y => y.StartDate)
            );

            GetListResponse<SearchAllBootcampListItemDto> response = _mapper.Map<GetListResponse<SearchAllBootcampListItemDto>>(
                bootcamps
            );
            return response;
        }
    }
}
