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
using static Application.Features.Blacklists.Constants.BlacklistsOperationClaims;

namespace Application.Features.Blacklists.Commands.Delete;

public class DeleteBlacklistCommand
    : IRequest<DeletedBlacklistResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBlacklists"];

    public class DeleteBlacklistCommandHandler : IRequestHandler<DeleteBlacklistCommand, DeletedBlacklistResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlacklistService _blacklistService;

        public DeleteBlacklistCommandHandler(IMapper mapper, IBlacklistService blacklistService)
        {
            _mapper = mapper;
            _blacklistService = blacklistService;
        }

        public async Task<DeletedBlacklistResponse> Handle(DeleteBlacklistCommand request, CancellationToken cancellationToken)
        {
            Blacklist? blacklist = await _blacklistService.GetAsync(
                predicate: b => b.Id == request.Id,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _blacklistService.DeleteAsync(blacklist!, request.IsPermament);

            DeletedBlacklistResponse response = _mapper.Map<DeletedBlacklistResponse>(blacklist);
            response.IsPermament = request.IsPermament;
            response.DeletedDate = request.IsPermament ? DateTime.UtcNow : response.DeletedDate;
            return response;
        }
    }
}
