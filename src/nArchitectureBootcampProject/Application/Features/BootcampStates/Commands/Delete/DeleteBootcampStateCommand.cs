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

namespace Application.Features.BootcampStates.Commands.Delete;

public class DeleteBootcampStateCommand
    : IRequest<DeletedBootcampStateResponse>,
        ISecuredRequest,
        ICacheRemoverRequest,
        ILoggableRequest,
        ITransactionalRequest
{
    public Guid Id { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [Admin, Write, BootcampStatesOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBootcampStates"];

    public class DeleteBootcampStateCommandHandler : IRequestHandler<DeleteBootcampStateCommand, DeletedBootcampStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampStateService _bootcampStateService;

        public DeleteBootcampStateCommandHandler(IMapper mapper, IBootcampStateService bootcampStateService)
        {
            _mapper = mapper;
            _bootcampStateService = bootcampStateService;
        }

        public async Task<DeletedBootcampStateResponse> Handle(
            DeleteBootcampStateCommand request,
            CancellationToken cancellationToken
        )
        {
            BootcampState? bootcampState = await _bootcampStateService.GetAsync(
                predicate: bs => bs.Id == request.Id,
                cancellationToken: cancellationToken
            );

            await _bootcampStateService.DeleteAsync(bootcampState!, request.IsPermament);

            DeletedBootcampStateResponse response = _mapper.Map<DeletedBootcampStateResponse>(bootcampState);
            response.IsPermament = request.IsPermament;
            return response;
        }
    }
}
