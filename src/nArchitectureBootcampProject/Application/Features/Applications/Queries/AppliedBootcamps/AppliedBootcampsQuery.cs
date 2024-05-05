using Application.Features.Applications.Rules;
using Application.Services.Applications;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Features.Applications.Queries.AppliedBootcamps;

public class AppliedBootcampsQuery : IRequest<GetListResponse<AppliedBootcampsResponse>>
{
    public Guid ApplicantId { get; set; }
    public PageRequest PageRequest { get; set; }

    public class AppliedBootcampsQueryHandler : IRequestHandler<AppliedBootcampsQuery, GetListResponse<AppliedBootcampsResponse>>
    {
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;
        private readonly ApplicationBusinessRules _applicationBusinessRules;

        public AppliedBootcampsQueryHandler(
            IApplicationService applicationService,
            IMapper mapper,
            ApplicationBusinessRules applicationBusinessRules
        )
        {
            _applicationService = applicationService;
            _mapper = mapper;
            _applicationBusinessRules = applicationBusinessRules;
        }

        public async Task<GetListResponse<AppliedBootcampsResponse>> Handle(
            AppliedBootcampsQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<ApplicationEntity>? applications = await _applicationService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                predicate: x => x.ApplicantId == request.ApplicantId,
                include: x =>
                    x.Include(x => x.Bootcamp)
                        .ThenInclude(bootcamp => bootcamp.Instructor)
                        .Include(x => x.Bootcamp)
                        .ThenInclude(bootcamp => bootcamp.BootcampState),
                orderBy: x => x.OrderByDescending(app => app.CreatedDate)
            );

            GetListResponse<AppliedBootcampsResponse> response = _mapper.Map<GetListResponse<AppliedBootcampsResponse>>(
                applications
            );

            return response;
        }
    }
}
