using Application.Services.Applications;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Applications.Constants.ApplicationsOperationClaims;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Features.Applications.Queries.GetListByInstructorDeleted;

public class GetListByInstructorDeletedApplicationQuery : IRequest<GetListResponse<GetListByInstructorDeletedApplicationListItemDto>>/*, ISecuredRequest , ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }
    public Guid InstructorId { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListApplications({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetApplications";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListByInstructorApplicationQueryHandler
        : IRequestHandler<GetListByInstructorDeletedApplicationQuery, GetListResponse<GetListByInstructorDeletedApplicationListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        public GetListByInstructorApplicationQueryHandler(IMapper mapper, IApplicationService applicationService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
        }

        public async Task<GetListResponse<GetListByInstructorDeletedApplicationListItemDto>> Handle(
            GetListByInstructorDeletedApplicationQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<ApplicationEntity>? applications = await _applicationService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                include: x => 
                    x.Include(x => x.Applicant)
                      .Include(x => x.Bootcamp).ThenInclude(x => x.Instructor)
                      .Include(x => x.ApplicationState),
                withDeleted: true,
                predicate: x => x.Bootcamp.InstructorId == request.InstructorId && x.DeletedDate != null
            );

            GetListResponse<GetListByInstructorDeletedApplicationListItemDto> response = _mapper.Map<
                GetListResponse<GetListByInstructorDeletedApplicationListItemDto>
            >(applications);
            return response;
        }
    }
}
