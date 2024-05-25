using Application.Features.BootcampStates.Constants;
using Application.Services.BootcampStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.BootcampStates.Constants.BootcampStatesOperationClaims;

namespace Application.Features.BootcampStates.Commands.Restore;

public class RestoreBootcampStateCommand : IRequest<RestoredBootcampStateResponse>
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
    public string[]? CacheGroupKey => ["GetBootcampStates"];

    public class RestoreBootcampStateCommandHandler : IRequestHandler<RestoreBootcampStateCommand, RestoredBootcampStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampStateService _bootcampStateService;

        public RestoreBootcampStateCommandHandler(IMapper mapper, IBootcampStateService bootcampStateService)
        {
            _mapper = mapper;
            _bootcampStateService = bootcampStateService;
        }

        public async Task<RestoredBootcampStateResponse> Handle(RestoreBootcampStateCommand request, CancellationToken cancellationToken)
        {
            BootcampState? bootcampState = await _bootcampStateService.GetAsync(
                predicate: b => b.Id == request.Id && b.DeletedDate != null,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _bootcampStateService.RestoreAsync(bootcampState!);

            RestoredBootcampStateResponse response = _mapper.Map<RestoredBootcampStateResponse>(bootcampState);
            return response;
        }
    }
}
