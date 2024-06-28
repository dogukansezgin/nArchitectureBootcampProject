using Application.Features.Applicants.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.BootcampStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.BootcampStates.Constants.BootcampStatesOperationClaims;

namespace Application.Features.BootcampStates.Queries.GetByName;

public class GetByNameBootcampStateQuery : IRequest<GetByNameBootcampStateResponse>, ISecuredRequest
{
    public string Name { get; set; }

    public string[] Roles =>
        [
            UsersOperationClaims.Admin,
            EmployeesOperationClaims.User,
            InstructorsOperationClaims.User,
            ApplicantsOperationClaims.User
        ];

    public class GetByIdBootcampStateQueryHandler : IRequestHandler<GetByNameBootcampStateQuery, GetByNameBootcampStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampStateService _bootcampStateService;

        public GetByIdBootcampStateQueryHandler(IMapper mapper, IBootcampStateService bootcampStateService)
        {
            _mapper = mapper;
            _bootcampStateService = bootcampStateService;
        }

        public async Task<GetByNameBootcampStateResponse> Handle(
            GetByNameBootcampStateQuery request,
            CancellationToken cancellationToken
        )
        {
            BootcampState? bootcampState = await _bootcampStateService.GetByNameAsync(request.Name);

            GetByNameBootcampStateResponse response = _mapper.Map<GetByNameBootcampStateResponse>(bootcampState);
            return response;
        }
    }
}
