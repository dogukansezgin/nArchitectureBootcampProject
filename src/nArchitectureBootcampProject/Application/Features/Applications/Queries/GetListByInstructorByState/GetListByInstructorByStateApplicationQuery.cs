using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
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

namespace Application.Features.Applications.Queries.GetListByInstructorByState;

public class GetListByInstructorByStateApplicationQuery
    : IRequest<GetListResponse<GetListByInstructorByStateApplicationListItemDto>>,
        ISecuredRequest /*, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }
    public Guid InstructorId { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, InstructorsOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListApplications({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetApplications";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListByInstructorByStateApplicationQueryHandler
        : IRequestHandler<
            GetListByInstructorByStateApplicationQuery,
            GetListResponse<GetListByInstructorByStateApplicationListItemDto>
        >
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        public GetListByInstructorByStateApplicationQueryHandler(IMapper mapper, IApplicationService applicationService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
        }

        public async Task<GetListResponse<GetListByInstructorByStateApplicationListItemDto>> Handle(
            GetListByInstructorByStateApplicationQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<ApplicationEntity>? applications = await _applicationService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                include: x =>
                    x.Include(x => x.Applicant)
                        .Include(x => x.Bootcamp)
                        .ThenInclude(x => x.Instructor)
                        .Include(x => x.ApplicationState),
                predicate: x => x.Bootcamp.InstructorId == request.InstructorId && x.ApplicationState.Name == "De�erlendirme"
            ); // Only data in "De�erlendirme" status.

            GetListResponse<GetListByInstructorByStateApplicationListItemDto> response = _mapper.Map<
                GetListResponse<GetListByInstructorByStateApplicationListItemDto>
            >(applications);
            return response;
        }
    }
}
