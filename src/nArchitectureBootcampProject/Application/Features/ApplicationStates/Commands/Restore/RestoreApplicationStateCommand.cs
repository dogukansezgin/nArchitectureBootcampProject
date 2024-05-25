using Application.Features.ApplicationStates.Constants;
using Application.Services.ApplicationStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.ApplicationStates.Constants.ApplicationStatesOperationClaims;

namespace Application.Features.ApplicationStates.Commands.Restore;

public class RestoreApplicationStateCommand : IRequest<RestoredApplicationStateResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Write];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplicationStates"];

    public class RestoreApplicationStateCommandHandler : IRequestHandler<RestoreApplicationStateCommand, RestoredApplicationStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationStateService _applicationStateService;

        public RestoreApplicationStateCommandHandler(IMapper mapper, IApplicationStateService applicationStateService)
        {
            _mapper = mapper;
            _applicationStateService = applicationStateService;
        }

        public async Task<RestoredApplicationStateResponse> Handle(RestoreApplicationStateCommand request, CancellationToken cancellationToken)
        {
            ApplicationState? applicationState = await _applicationStateService.GetAsync(
                predicate: b => b.Id == request.Id && b.DeletedDate != null,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _applicationStateService.RestoreAsync(applicationState!);

            RestoredApplicationStateResponse response = _mapper.Map<RestoredApplicationStateResponse>(applicationState);
            return response;
        }
    }
}
