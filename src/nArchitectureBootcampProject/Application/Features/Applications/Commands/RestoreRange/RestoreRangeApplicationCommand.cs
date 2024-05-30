using Application.Features.Applications.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.Applications;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Applications.Constants.ApplicationsOperationClaims;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Features.Applications.Commands.RestoreRange;

public class RestoreRangeApplicationCommand : IRequest<RestoredRangeApplicationResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetApplications" };

    public class RestoreRangeApplicationCommandHandler
        : IRequestHandler<RestoreRangeApplicationCommand, RestoredRangeApplicationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        public RestoreRangeApplicationCommandHandler(IMapper mapper, IApplicationService applicationService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
        }

        public async Task<RestoredRangeApplicationResponse> Handle(
            RestoreRangeApplicationCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<ApplicationEntity>? applications = await _applicationService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _applicationService.RestoreRangeAsync(applications.Items);

            RestoredRangeApplicationResponse response = new RestoredRangeApplicationResponse
            {
                Ids = applications.Items.Select(b => b.Id).ToList()
            };

            return response;
        }
    }
}
