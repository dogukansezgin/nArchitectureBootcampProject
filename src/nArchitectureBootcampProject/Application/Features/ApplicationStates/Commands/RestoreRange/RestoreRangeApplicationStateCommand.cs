using Application.Features.ApplicationStates.Constants;
using Application.Services.ApplicationStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.ApplicationStates.Constants.ApplicationStatesOperationClaims;

namespace Application.Features.ApplicationStates.Commands.RestoreRange;

public class RestoreRangeApplicationStateCommand : IRequest<RestoredRangeApplicationStateResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }

    public string[] Roles => [Admin, Write];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetApplicationStates" };

    public class RestoreRangeApplicationStateCommandHandler : IRequestHandler<RestoreRangeApplicationStateCommand, RestoredRangeApplicationStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationStateService _applicationStateService;

        public RestoreRangeApplicationStateCommandHandler(IMapper mapper, IApplicationStateService applicationStateService)
        {
            _mapper = mapper;
            _applicationStateService = applicationStateService;
        }

        public async Task<RestoredRangeApplicationStateResponse> Handle(
            RestoreRangeApplicationStateCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<ApplicationState>? applicationStates = await _applicationStateService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _applicationStateService.RestoreRangeAsync(applicationStates.Items);

            RestoredRangeApplicationStateResponse response = new RestoredRangeApplicationStateResponse
            {
                Ids = applicationStates.Items.Select(b => b.Id).ToList()
            };

            return response;
        }
    }
}
