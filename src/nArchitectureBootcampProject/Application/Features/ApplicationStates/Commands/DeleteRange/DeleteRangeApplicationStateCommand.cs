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

namespace Application.Features.ApplicationStates.Commands.DeleteRange;

public class DeleteRangeApplicationStateCommand : IRequest<DeletedRangeApplicationStateResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [Admin, Write, ApplicationStatesOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetApplicationStates" };

    public class DeleteRangeApplicationStateCommandHandler
        : IRequestHandler<DeleteRangeApplicationStateCommand, DeletedRangeApplicationStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationStateService _applicationStateService;

        public DeleteRangeApplicationStateCommandHandler(IMapper mapper, IApplicationStateService applicationStateService)
        {
            _mapper = mapper;
            _applicationStateService = applicationStateService;
        }

        public async Task<DeletedRangeApplicationStateResponse> Handle(
            DeleteRangeApplicationStateCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<ApplicationState>? applicationStates = await _applicationStateService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _applicationStateService.DeleteRangeAsync(applicationStates.Items, request.IsPermament);

            DeletedRangeApplicationStateResponse response = new DeletedRangeApplicationStateResponse
            {
                Ids = applicationStates.Items.Select(b => b.Id).ToList(),
                DeletedDate = DateTime.UtcNow,
                IsPermament = request.IsPermament
            };

            return response;
        }
    }
}
