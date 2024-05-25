using Application.Features.Applications.Constants;
using Application.Services.Applications;
using AutoMapper;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Applications.Constants.ApplicationsOperationClaims;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Features.Applications.Commands.DeleteRange;

public class DeleteRangeApplicationCommand : IRequest<DeletedRangeApplicationResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [Admin, Write, ApplicationsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetApplications" };

    public class DeleteRangeApplicationCommandHandler
        : IRequestHandler<DeleteRangeApplicationCommand, DeletedRangeApplicationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        public DeleteRangeApplicationCommandHandler(IMapper mapper, IApplicationService applicationService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
        }

        public async Task<DeletedRangeApplicationResponse> Handle(
            DeleteRangeApplicationCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<ApplicationEntity>? applications = await _applicationService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _applicationService.DeleteRangeAsync(applications.Items, request.IsPermament);

            DeletedRangeApplicationResponse response = new DeletedRangeApplicationResponse
            {
                Ids = applications.Items.Select(b => b.Id).ToList(),
                DeletedDate = DateTime.UtcNow,
                IsPermament = request.IsPermament
            };

            return response;
        }
    }
}
