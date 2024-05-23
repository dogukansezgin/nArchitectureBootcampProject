using Application.Features.Applications.Commands.Delete;
using Application.Features.Applications.Constants;
using Application.Services.Applications;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Applications.Constants.ApplicationsOperationClaims;

namespace Application.Features.Applications.Commands.DeleteSelected;
public class DeleteSelectedApplicationCommand
    : IRequest<DeletedSelectedApplicationResponse>
    , ISecuredRequest
{
    public ICollection<Guid> Ids { get; set; }

    public string[] Roles => [Admin, Write, ApplicationsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplications"];

    public class DeleteSelectedApplicationCommandHandler : IRequestHandler<DeleteSelectedApplicationCommand, DeletedSelectedApplicationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;
        public DeleteSelectedApplicationCommandHandler(IMapper mapper, IApplicationService applicationService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
        }
        public async Task<DeletedSelectedApplicationResponse> Handle(
            DeleteSelectedApplicationCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Domain.Entities.Application>? applications = await _applicationService.GetListAsync(
                size: request.Ids.Count,
                predicate: a => request.Ids.Contains(a.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            //await _applicationService.DeleteAsync(application!, request.IsPermament);
            await _applicationService.DeleteRangeAsync(applications.Items, true);

            DeletedSelectedApplicationResponse response = new DeletedSelectedApplicationResponse
            {
                Ids = applications.Items.Select(a => a.Id).ToList(),
                DeletedDate = DateTime.UtcNow
            };
            return response;
        }

    }
}
