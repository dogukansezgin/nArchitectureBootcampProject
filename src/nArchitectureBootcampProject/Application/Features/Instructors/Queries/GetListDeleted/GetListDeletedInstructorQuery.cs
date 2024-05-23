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

namespace Application.Features.Instructors.Queries.GetListDeleted;

public class GetListDeletedInstructorQuery : IRequest<GetListResponse<GetListDeletedInstructorListItemDto>> /*, ISecuredRequest, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListInstructors({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetInstructors";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListDeletedInstructorQueryHandler
        : IRequestHandler<GetListDeletedInstructorQuery, GetListResponse<GetListDeletedInstructorListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorService _instructorService;

        public GetListDeletedInstructorQueryHandler(IMapper mapper, IInstructorService instructorService)
        {
            _mapper = mapper;
            _instructorService = instructorService;
        }

        public async Task<GetListResponse<GetListDeletedInstructorListItemDto>> Handle(
            GetListDeletedInstructorQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Instructor>? instructors = await _instructorService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                withDeleted: true,
                predicate: x => x.DeletedDate != null
            );

            GetListResponse<GetListDeletedInstructorListItemDto> response = _mapper.Map<GetListResponse<GetListDeletedInstructorListItemDto>>(
                instructors
            );
            return response;
        }
    }
}
