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

namespace Application.Features.ApplicationStates.Commands.Delete;

public class DeleteApplicationStateCommand : IRequest<DeletedApplicationStateResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplicationStates"];

    public class DeleteApplicationStateCommandHandler
        : IRequestHandler<DeleteApplicationStateCommand, DeletedApplicationStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationStateService _applicationStateService;

        public DeleteApplicationStateCommandHandler(IMapper mapper, IApplicationStateService applicationStateService)
        {
            _mapper = mapper;
            _applicationStateService = applicationStateService;
        }

        public async Task<DeletedApplicationStateResponse> Handle(
            DeleteApplicationStateCommand request,
            CancellationToken cancellationToken
        )
        {
            ApplicationState? applicationState = await _applicationStateService.GetAsync(
                predicate: a => a.Id == request.Id,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _applicationStateService.DeleteAsync(applicationState!, request.IsPermament);

            DeletedApplicationStateResponse response = _mapper.Map<DeletedApplicationStateResponse>(applicationState);
            response.IsPermament = request.IsPermament;
            response.DeletedDate = request.IsPermament ? DateTime.UtcNow : response.DeletedDate;
            return response;
        }
    }
}
