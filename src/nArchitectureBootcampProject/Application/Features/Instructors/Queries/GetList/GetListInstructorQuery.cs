using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.Instructors;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Instructors.Constants.InstructorsOperationClaims;

namespace Application.Features.Instructors.Queries.GetList;

public class GetListInstructorQuery : IRequest<GetListResponse<GetListInstructorListItemDto>>, ISecuredRequest /*, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListInstructors({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetInstructors";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListInstructorQueryHandler
        : IRequestHandler<GetListInstructorQuery, GetListResponse<GetListInstructorListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorService _instructorService;

        public GetListInstructorQueryHandler(IMapper mapper, IInstructorService instructorService)
        {
            _mapper = mapper;
            _instructorService = instructorService;
        }

        public async Task<GetListResponse<GetListInstructorListItemDto>> Handle(
            GetListInstructorQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Instructor>? instructors = await _instructorService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListInstructorListItemDto> response = _mapper.Map<GetListResponse<GetListInstructorListItemDto>>(
                instructors
            );
            return response;
        }
    }
}
