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

namespace Application.Features.ApplicationStates.Queries.GetById;

public class GetByIdApplicationStateQuery : IRequest<GetByIdApplicationStateResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User, ApplicantsOperationClaims.User];

    public class GetByIdApplicationStateQueryHandler
        : IRequestHandler<GetByIdApplicationStateQuery, GetByIdApplicationStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationStateService _applicationStateService;

        public GetByIdApplicationStateQueryHandler(IMapper mapper, IApplicationStateService applicationStateService)
        {
            _mapper = mapper;
            _applicationStateService = applicationStateService;
        }

        public async Task<GetByIdApplicationStateResponse> Handle(
            GetByIdApplicationStateQuery request,
            CancellationToken cancellationToken
        )
        {
            ApplicationState? applicationState = await _applicationStateService.GetByIdAsync(request.Id);

            GetByIdApplicationStateResponse response = _mapper.Map<GetByIdApplicationStateResponse>(applicationState);
            return response;
        }
    }
}
