using Application.Features.Blacklists.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
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

namespace Application.Features.Blacklists.Commands.DeleteRange;

public class DeleteRangeBlacklistCommand : IRequest<DeletedRangeBlacklistResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetBlacklists" };

    public class DeleteRangeBlacklistCommandHandler
        : IRequestHandler<DeleteRangeBlacklistCommand, DeletedRangeBlacklistResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlacklistService _blacklistService;

        public DeleteRangeBlacklistCommandHandler(IMapper mapper, IBlacklistService blacklistService)
        {
            _mapper = mapper;
            _blacklistService = blacklistService;
        }

        public async Task<DeletedRangeBlacklistResponse> Handle(
            DeleteRangeBlacklistCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Blacklist>? blacklists = await _blacklistService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _blacklistService.DeleteRangeAsync(blacklists.Items, request.IsPermament);

            DeletedRangeBlacklistResponse response = new DeletedRangeBlacklistResponse
            {
                Ids = blacklists.Items.Select(b => b.Id).ToList(),
                DeletedDate = DateTime.UtcNow,
                IsPermament = request.IsPermament
            };

            return response;
        }
    }
}
