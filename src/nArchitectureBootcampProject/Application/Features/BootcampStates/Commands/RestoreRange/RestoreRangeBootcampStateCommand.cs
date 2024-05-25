using Application.Features.BootcampStates.Constants;
using Application.Services.BootcampStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.BootcampStates.Constants.BootcampStatesOperationClaims;

namespace Application.Features.BootcampStates.Commands.RestoreRange;

public class RestoreRangeBootcampStateCommand : IRequest<RestoredRangeBootcampStateResponse>
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
    public string[]? CacheGroupKey => new[] { "GetBootcampStates" };

    public class RestoreRangeBootcampStateCommandHandler
        : IRequestHandler<RestoreRangeBootcampStateCommand, RestoredRangeBootcampStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampStateService _bootcampStateService;

        public RestoreRangeBootcampStateCommandHandler(IMapper mapper, IBootcampStateService bootcampStateService)
        {
            _mapper = mapper;
            _bootcampStateService = bootcampStateService;
        }

        public async Task<RestoredRangeBootcampStateResponse> Handle(
            RestoreRangeBootcampStateCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<BootcampState>? bootcampStates = await _bootcampStateService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _bootcampStateService.RestoreRangeAsync(bootcampStates.Items);

            RestoredRangeBootcampStateResponse response = new RestoredRangeBootcampStateResponse
            {
                Ids = bootcampStates.Items.Select(b => b.Id).ToList()
            };

            return response;
        }
    }
}
