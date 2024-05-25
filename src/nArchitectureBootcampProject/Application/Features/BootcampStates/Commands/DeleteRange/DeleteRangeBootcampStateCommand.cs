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

namespace Application.Features.BootcampStates.Commands.DeleteRange;

public class DeleteRangeBootcampStateCommand : IRequest<DeletedRangeBootcampStateResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [Admin, Write, BootcampStatesOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetBootcampStates" };

    public class DeleteRangeBootcampStateCommandHandler
        : IRequestHandler<DeleteRangeBootcampStateCommand, DeletedRangeBootcampStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampStateService _bootcampStateService;

        public DeleteRangeBootcampStateCommandHandler(IMapper mapper, IBootcampStateService bootcampStateService)
        {
            _mapper = mapper;
            _bootcampStateService = bootcampStateService;
        }

        public async Task<DeletedRangeBootcampStateResponse> Handle(
            DeleteRangeBootcampStateCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<BootcampState>? bootcampStates = await _bootcampStateService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _bootcampStateService.DeleteRangeAsync(bootcampStates.Items, request.IsPermament);

            DeletedRangeBootcampStateResponse response = new DeletedRangeBootcampStateResponse
            {
                Ids = bootcampStates.Items.Select(b => b.Id).ToList(),
                DeletedDate = DateTime.UtcNow,
                IsPermament = request.IsPermament
            };

            return response;
        }
    }
}
