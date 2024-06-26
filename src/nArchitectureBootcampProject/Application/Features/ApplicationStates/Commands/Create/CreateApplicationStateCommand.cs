using Application.Features.ApplicationStates.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.ApplicationStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.ApplicationStates.Constants.ApplicationStatesOperationClaims;

namespace Application.Features.ApplicationStates.Commands.Create;

public class CreateApplicationStateCommand : IRequest<CreatedApplicationStateResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public string Name { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplicationStates"];

    public class CreateApplicationStateCommandHandler
        : IRequestHandler<CreateApplicationStateCommand, CreatedApplicationStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationStateService _applicationStateService;

        public CreateApplicationStateCommandHandler(IMapper mapper, IApplicationStateService applicationStateService)
        {
            _mapper = mapper;
            _applicationStateService = applicationStateService;
        }

        public async Task<CreatedApplicationStateResponse> Handle(
            CreateApplicationStateCommand request,
            CancellationToken cancellationToken
        )
        {
            ApplicationState applicationState = _mapper.Map<ApplicationState>(request);

            await _applicationStateService.AddAsync(applicationState);

            CreatedApplicationStateResponse response = _mapper.Map<CreatedApplicationStateResponse>(applicationState);
            return response;
        }
    }
}
