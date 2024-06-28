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

namespace Application.Features.Bootcamps.Queries.GetListDeleted;

public class GetListDeletedBootcampQuery : IRequest<GetListResponse<GetListDeletedBootcampListItemDto>>, ISecuredRequest /*, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListBootcamps({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetBootcamps";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListDeletedBootcampQueryHandler
        : IRequestHandler<GetListDeletedBootcampQuery, GetListResponse<GetListDeletedBootcampListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public GetListDeletedBootcampQueryHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<GetListResponse<GetListDeletedBootcampListItemDto>> Handle(
            GetListDeletedBootcampQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Bootcamp>? bootcamps = await _bootcampService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                orderBy: x => x.OrderByDescending(y => y.StartDate),
                include: x => x.Include(x => x.Instructor).Include(x => x.BootcampState),
                withDeleted: true,
                predicate: x => x.DeletedDate != null
            );

            GetListResponse<GetListDeletedBootcampListItemDto> response = _mapper.Map<
                GetListResponse<GetListDeletedBootcampListItemDto>
            >(bootcamps);
            return response;
        }
    }
}
