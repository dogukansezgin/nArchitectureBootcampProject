using Application.Features.BootcampStates.Constants;
using Application.Features.BootcampStates.Rules;
using Application.Services.BootcampStates;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.BootcampStates.Constants.BootcampStatesOperationClaims;

namespace Application.Features.BootcampStates.Commands.Create;

public class CreateBootcampStateCommand
    : IRequest<CreatedBootcampStateResponse>
    //,
    //    ISecuredRequest,
    //    ICacheRemoverRequest,
    //    ILoggableRequest,
    //    ITransactionalRequest
{
    public string Name { get; set; }

    public string[] Roles => [Admin, Write, BootcampStatesOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBootcampStates"];

    public class CreateBootcampStateCommandHandler : IRequestHandler<CreateBootcampStateCommand, CreatedBootcampStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampStateService _bootcampStateService;

        public CreateBootcampStateCommandHandler(IMapper mapper, IBootcampStateService bootcampStateService)
        {
            _mapper = mapper;
            _bootcampStateService = bootcampStateService;
        }

        public async Task<CreatedBootcampStateResponse> Handle(
            CreateBootcampStateCommand request,
            CancellationToken cancellationToken
        )
        {
            BootcampState bootcampState = _mapper.Map<BootcampState>(request);

            await _bootcampStateService.AddAsync(bootcampState);

            CreatedBootcampStateResponse response = _mapper.Map<CreatedBootcampStateResponse>(bootcampState);
            return response;
        }
    }
}
