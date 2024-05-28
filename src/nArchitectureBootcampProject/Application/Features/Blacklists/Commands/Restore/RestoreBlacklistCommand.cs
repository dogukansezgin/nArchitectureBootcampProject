using Application.Features.Blacklists.Constants;
using Application.Services.Blacklists;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Blacklists.Constants.BlacklistsOperationClaims;

namespace Application.Features.Blacklists.Commands.Restore;

public class RestoreBlacklistCommand : IRequest<RestoredBlacklistResponse>
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
    public string[]? CacheGroupKey => ["GetBlacklists"];

    public class RestoreBlacklistCommandHandler : IRequestHandler<RestoreBlacklistCommand, RestoredBlacklistResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlacklistService _blacklistService;

        public RestoreBlacklistCommandHandler(IMapper mapper, IBlacklistService blacklistService)
        {
            _mapper = mapper;
            _blacklistService = blacklistService;
        }

        public async Task<RestoredBlacklistResponse> Handle(
            RestoreBlacklistCommand request,
            CancellationToken cancellationToken
        )
        {
            Blacklist? blacklist = await _blacklistService.GetAsync(
                predicate: b => b.Id == request.Id && b.DeletedDate != null,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _blacklistService.RestoreAsync(blacklist!);

            RestoredBlacklistResponse response = _mapper.Map<RestoredBlacklistResponse>(blacklist);
            return response;
        }
    }
}
