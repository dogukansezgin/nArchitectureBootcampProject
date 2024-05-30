using Application.Features.BootcampStates.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.BootcampStates;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.BootcampStates.Constants.BootcampStatesOperationClaims;

namespace Application.Features.BootcampStates.Commands.Update;

public class UpdateBootcampStateCommand : IRequest<UpdatedBootcampStateResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBootcampStates"];

    public class UpdateBootcampStateCommandHandler : IRequestHandler<UpdateBootcampStateCommand, UpdatedBootcampStateResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampStateService _bootcampStateService;

        public UpdateBootcampStateCommandHandler(IMapper mapper, IBootcampStateService bootcampStateService)
        {
            _mapper = mapper;
            _bootcampStateService = bootcampStateService;
        }

        public async Task<UpdatedBootcampStateResponse> Handle(
            UpdateBootcampStateCommand request,
            CancellationToken cancellationToken
        )
        {
            BootcampState? bootcampState = await _bootcampStateService.GetAsync(
                predicate: bs => bs.Id == request.Id,
                cancellationToken: cancellationToken
            );

            bootcampState = _mapper.Map(request, bootcampState);

            await _bootcampStateService.UpdateAsync(bootcampState!);

            UpdatedBootcampStateResponse response = _mapper.Map<UpdatedBootcampStateResponse>(bootcampState);
            return response;
        }
    }
}
