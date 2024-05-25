using Application.Features.ApplicationStates.Constants;
using Application.Features.ApplicationStates.Rules;
using Application.Services.ApplicationStates;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.ApplicationStates.Constants.ApplicationStatesOperationClaims;

namespace Application.Features.ApplicationStates.Commands.Update;

public class UpdateApplicationStateCommand
    : IRequest<UpdatedApplicationStateResponse>
    //,
    //    ISecuredRequest,
    //    ICacheRemoverRequest,
    //    ILoggableRequest,
    //    ITransactionalRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public string[] Roles => [Admin, Write, ApplicationStatesOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplicationStates"];

    public class UpdateApplicationStateCommandHandler
        : IRequestHandler<UpdateApplicationStateCommand, UpdatedApplicationStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationStateService _applicationStateService;

        public UpdateApplicationStateCommandHandler(IMapper mapper, IApplicationStateService applicationStateService)
        {
            _mapper = mapper;
            _applicationStateService = applicationStateService;
        }

        public async Task<UpdatedApplicationStateResponse> Handle(
            UpdateApplicationStateCommand request,
            CancellationToken cancellationToken
        )
        {
            ApplicationState? applicationState = await _applicationStateService.GetAsync(
                predicate: a => a.Id == request.Id,
                cancellationToken: cancellationToken
            );

            applicationState = _mapper.Map(request, applicationState);

            await _applicationStateService.UpdateAsync(applicationState!);

            UpdatedApplicationStateResponse response = _mapper.Map<UpdatedApplicationStateResponse>(applicationState);
            return response;
        }
    }
}
