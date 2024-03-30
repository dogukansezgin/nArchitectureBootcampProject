using Application.Services.Blacklists;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Blacklists.Constants.BlacklistsOperationClaims;

namespace Application.Features.Blacklists.Queries.GetById;

public class GetByIdBlacklistQuery : IRequest<GetByIdBlacklistResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdBlacklistQueryHandler : IRequestHandler<GetByIdBlacklistQuery, GetByIdBlacklistResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlacklistService _blacklistService;

        public GetByIdBlacklistQueryHandler(IMapper mapper, IBlacklistService blacklistService)
        {
            _mapper = mapper;
            _blacklistService = blacklistService;
        }

        public async Task<GetByIdBlacklistResponse> Handle(GetByIdBlacklistQuery request, CancellationToken cancellationToken)
        {
            Blacklist? blacklist = await _blacklistService.GetByIdAsync(request.Id);

            GetByIdBlacklistResponse response = _mapper.Map<GetByIdBlacklistResponse>(blacklist);
            return response;
        }
    }
}
