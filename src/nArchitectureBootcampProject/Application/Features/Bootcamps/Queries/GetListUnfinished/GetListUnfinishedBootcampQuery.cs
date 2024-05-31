using Application.Features.Applicants.Constants;
using Application.Features.Bootcamps.Queries.GetList;
using Application.Features.Bootcamps.Queries.GetListUnfinished;
using Application.Features.Employees.Constants;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.Bootcamps;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Bootcamps.Constants.BootcampsOperationClaims;

namespace Application.Features.Bootcamps.Queries.GetAllList;

public class GetListUnfinishedBootcampQuery
    : IRequest<
        GetListResponse<GetListUnfinishedBootcampListItemDto>
    >/*, ISecuredRequest, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListBootcamps({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetBootcamps";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListUnfinishedBootcampQueryHandler
        : IRequestHandler<GetListUnfinishedBootcampQuery, GetListResponse<GetListUnfinishedBootcampListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public GetListUnfinishedBootcampQueryHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<GetListResponse<GetListUnfinishedBootcampListItemDto>> Handle(
            GetListUnfinishedBootcampQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Bootcamp>? bootcamps = await _bootcampService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                predicate: x => x.StartDate >= DateTime.Now.AddDays(15),
                include: x => x.Include(x => x.Instructor).Include(x => x.BootcampState).Include(x => x.BootcampImages)
            );

            GetListResponse<GetListUnfinishedBootcampListItemDto> response = _mapper.Map<
                GetListResponse<GetListUnfinishedBootcampListItemDto>
            >(bootcamps);
            return response;
        }
    }
}
