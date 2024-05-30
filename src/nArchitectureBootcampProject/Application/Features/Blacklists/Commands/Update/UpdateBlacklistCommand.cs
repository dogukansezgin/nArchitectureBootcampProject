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

namespace Application.Features.Blacklists.Commands.Update;

public class UpdateBlacklistCommand
    : IRequest<UpdatedBlacklistResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public string Reason { get; set; }
    //public DateTime Date { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBlacklists"];

    public class UpdateBlacklistCommandHandler : IRequestHandler<UpdateBlacklistCommand, UpdatedBlacklistResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlacklistService _blacklistService;

        public UpdateBlacklistCommandHandler(IMapper mapper, IBlacklistService blacklistService)
        {
            _mapper = mapper;
            _blacklistService = blacklistService;
        }

        public async Task<UpdatedBlacklistResponse> Handle(UpdateBlacklistCommand request, CancellationToken cancellationToken)
        {
            Blacklist? blacklist = await _blacklistService.GetAsync(
                predicate: b => b.Id == request.Id,
                cancellationToken: cancellationToken
            );

            blacklist = _mapper.Map(request, blacklist);

            await _blacklistService.UpdateAsync(blacklist!);

            UpdatedBlacklistResponse response = _mapper.Map<UpdatedBlacklistResponse>(blacklist);
            return response;
        }
    }
}
