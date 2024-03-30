using Application.Services.BootcampStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.BootcampStates.Constants.BootcampStatesOperationClaims;

namespace Application.Features.BootcampStates.Queries.GetById;

public class GetByIdBootcampStateQuery : IRequest<GetByIdBootcampStateResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdBootcampStateQueryHandler : IRequestHandler<GetByIdBootcampStateQuery, GetByIdBootcampStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampStateService _bootcampStateService;

        public GetByIdBootcampStateQueryHandler(IMapper mapper, IBootcampStateService bootcampStateService)
        {
            _mapper = mapper;
            _bootcampStateService = bootcampStateService;
        }

        public async Task<GetByIdBootcampStateResponse> Handle(
            GetByIdBootcampStateQuery request,
            CancellationToken cancellationToken
        )
        {
            BootcampState? bootcampState = await _bootcampStateService.GetByIdAsync(request.Id);

            GetByIdBootcampStateResponse response = _mapper.Map<GetByIdBootcampStateResponse>(bootcampState);
            return response;
        }
    }
}
