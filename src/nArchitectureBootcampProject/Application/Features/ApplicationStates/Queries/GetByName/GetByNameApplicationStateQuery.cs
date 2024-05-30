using Application.Features.Applicants.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.ApplicationStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.ApplicationStates.Constants.ApplicationStatesOperationClaims;

namespace Application.Features.ApplicationStates.Queries.GetByName;

public class GetByNameApplicationStateQuery : IRequest<GetByNameApplicationStateResponse>, ISecuredRequest
{
    public string Name { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User, ApplicantsOperationClaims.User];

    public class GetByNameApplicationStateQueryHandler
        : IRequestHandler<GetByNameApplicationStateQuery, GetByNameApplicationStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationStateService _applicationStateService;

        public GetByNameApplicationStateQueryHandler(IMapper mapper, IApplicationStateService applicationStateService)
        {
            _mapper = mapper;
            _applicationStateService = applicationStateService;
        }

        public async Task<GetByNameApplicationStateResponse> Handle(
            GetByNameApplicationStateQuery request,
            CancellationToken cancellationToken
        )
        {
            ApplicationState? applicationState = await _applicationStateService.GetByNameAsync(request.Name.Trim());

            GetByNameApplicationStateResponse response = _mapper.Map<GetByNameApplicationStateResponse>(applicationState);
            return response;
        }
    }
}
