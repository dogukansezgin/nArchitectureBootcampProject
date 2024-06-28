using Application.Features.Applicants.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.Blacklists;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Blacklists.Constants.BlacklistsOperationClaims;

namespace Application.Features.Blacklists.Queries.GetByApplicantId;

public class GetByApplicantIdBlacklistQuery : IRequest<GetByApplicantIdBlacklistResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, ApplicantsOperationClaims.User];

    public class GetByIdBlacklistQueryHandler : IRequestHandler<GetByApplicantIdBlacklistQuery, GetByApplicantIdBlacklistResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlacklistService _blacklistService;

        public GetByIdBlacklistQueryHandler(IMapper mapper, IBlacklistService blacklistService)
        {
            _mapper = mapper;
            _blacklistService = blacklistService;
        }

        public async Task<GetByApplicantIdBlacklistResponse> Handle(
            GetByApplicantIdBlacklistQuery request,
            CancellationToken cancellationToken
        )
        {
            Blacklist? blacklist = await _blacklistService.GetByApplicantIdAsync(request.Id);

            GetByApplicantIdBlacklistResponse response = _mapper.Map<GetByApplicantIdBlacklistResponse>(blacklist);
            return response;
        }
    }
}
