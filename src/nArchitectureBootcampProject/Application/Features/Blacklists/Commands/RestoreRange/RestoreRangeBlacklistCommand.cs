using Application.Features.Blacklists.Constants;
using Application.Services.Blacklists;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Blacklists.Constants.BlacklistsOperationClaims;

namespace Application.Features.Blacklists.Commands.RestoreRange;

public class RestoreRangeBlacklistCommand : IRequest<RestoredRangeBlacklistResponse>
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
    public string[]? CacheGroupKey => new[] { "GetBlacklists" };

    public class RestoreRangeBlacklistCommandHandler
        : IRequestHandler<RestoreRangeBlacklistCommand, RestoredRangeBlacklistResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlacklistService _blacklistService;

        public RestoreRangeBlacklistCommandHandler(IMapper mapper, IBlacklistService blacklistService)
        {
            _mapper = mapper;
            _blacklistService = blacklistService;
        }

        public async Task<RestoredRangeBlacklistResponse> Handle(
            RestoreRangeBlacklistCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Blacklist>? blacklists = await _blacklistService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _blacklistService.RestoreRangeAsync(blacklists.Items);

            RestoredRangeBlacklistResponse response = new RestoredRangeBlacklistResponse
            {
                Ids = blacklists.Items.Select(b => b.Id).ToList()
            };

            return response;
        }
    }
}
