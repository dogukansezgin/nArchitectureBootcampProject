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

namespace Application.Features.Blacklists.Commands.Create;

public class CreateBlacklistCommand
    : IRequest<CreatedBlacklistResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid ApplicantId { get; set; }
    public string Reason { get; set; }
    //public DateTime Date { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBlacklists"];

    public class CreateBlacklistCommandHandler : IRequestHandler<CreateBlacklistCommand, CreatedBlacklistResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlacklistService _blacklistService;

        public CreateBlacklistCommandHandler(IMapper mapper, IBlacklistService blacklistService)
        {
            _mapper = mapper;
            _blacklistService = blacklistService;
        }

        public async Task<CreatedBlacklistResponse> Handle(CreateBlacklistCommand request, CancellationToken cancellationToken)
        {
            Blacklist blacklist = _mapper.Map<Blacklist>(request);
            blacklist.Date = DateTime.UtcNow;

            await _blacklistService.AddAsync(blacklist);

            CreatedBlacklistResponse response = _mapper.Map<CreatedBlacklistResponse>(blacklist);
            return response;
        }
    }
}
