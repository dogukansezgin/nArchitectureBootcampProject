using Application.Features.Applications.Commands.DeleteRange;
using Application.Features.Applications.Commands.Update;
using Application.Features.Applications.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.Applications;
using AutoMapper;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Applications.Constants.ApplicationsOperationClaims;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Features.Applications.Commands.UpdateRange;

public class UpdateRangeApplicationCommand : IRequest<UpdatedRangeApplicationResponse>, ISecuredRequest
//ICacheRemoverRequest,
//ILoggableRequest,
//ITransactionalRequest
{
    public List<UpdateApplicationCommand> Applications { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplications"];

    public class UpdateApplicationCommandHandler : IRequestHandler<UpdateRangeApplicationCommand, UpdatedRangeApplicationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        public UpdateApplicationCommandHandler(IMapper mapper, IApplicationService applicationService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
        }

        public async Task<UpdatedRangeApplicationResponse> Handle(
            UpdateRangeApplicationCommand request,
            CancellationToken cancellationToken
        )
        {
            var applicationIds = request.Applications.Select(a => a.Id).ToList();

            IPaginate<ApplicationEntity>? applications = await _applicationService.GetListAsync(
                size: request.Applications.Count,
                predicate: x => applicationIds.Contains(x.Id),
                cancellationToken: cancellationToken
            );

            foreach (var application in applications.Items)
            {
                var updateApplicationCommand = request.Applications.First(a => a.Id == application.Id);
                _mapper.Map(updateApplicationCommand, application);
            }

            await _applicationService.UpdateRangeAsync(applications.Items);

            UpdatedRangeApplicationResponse response = new UpdatedRangeApplicationResponse
            {
                Ids = applications.Items.Select(b => b.Id).ToList(),
                UpdatedDate = DateTime.UtcNow
            };

            return response;
        }
    }
}
