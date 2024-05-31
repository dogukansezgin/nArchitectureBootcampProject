using Application.Features.Applicants.Constants;
using Application.Features.Bootcamps.Queries.GetAllList;
using Application.Features.Bootcamps.Queries.GetList;
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

namespace Application.Features.Bootcamps.Queries.GetListFinished;

public class GetListFinishedBootcampQuery
    : IRequest<
        GetListResponse<GetListBootcampListItemDto>
    > /*, ISecuredRequest*/ /*, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListBootcamps({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetBootcamps";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListFinishedBootcampQueryHandler
        : IRequestHandler<GetListFinishedBootcampQuery, GetListResponse<GetListFinishedBootcampListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public GetListFinishedBootcampQueryHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<GetListResponse<GetListFinishedBootcampListItemDto>> Handle(
            GetListFinishedBootcampQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Bootcamp>? bootcamps = await _bootcampService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                predicate: x => x.StartDate < DateTime.Now.AddDays(15),
                include: x => x.Include(x => x.Instructor).Include(x => x.BootcampState).Include(x => x.BootcampImages)
            );

            GetListResponse<GetListFinishedBootcampListItemDto> response = _mapper.Map<
                GetListResponse<GetListFinishedBootcampListItemDto>
            >(bootcamps);
            return response;
        }
    }
}
