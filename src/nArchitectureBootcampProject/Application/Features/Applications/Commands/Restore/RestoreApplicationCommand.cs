using Application.Features.Applications.Constants;
using Application.Services.Applications;
using AutoMapper;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Applications.Constants.ApplicationsOperationClaims;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Features.Applications.Commands.Restore;

public class RestoreApplicationCommand : IRequest<RestoredApplicationResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Write];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplications"];

    public class RestoreApplicationCommandHandler : IRequestHandler<RestoreApplicationCommand, RestoredApplicationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        public RestoreApplicationCommandHandler(IMapper mapper, IApplicationService applicationService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
        }

        public async Task<RestoredApplicationResponse> Handle(RestoreApplicationCommand request, CancellationToken cancellationToken)
        {
            ApplicationEntity? application = await _applicationService.GetAsync(
                predicate: b => b.Id == request.Id && b.DeletedDate != null,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _applicationService.RestoreAsync(application!);

            RestoredApplicationResponse response = _mapper.Map<RestoredApplicationResponse>(application);
            return response;
        }
    }
}
