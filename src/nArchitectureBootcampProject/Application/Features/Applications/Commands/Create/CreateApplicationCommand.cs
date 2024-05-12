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

namespace Application.Features.Applications.Commands.Create;

public class CreateApplicationCommand
    : IRequest<CreatedApplicationResponse>,
        ISecuredRequest,
        //ICacheRemoverRequest,
        ILoggableRequest,
        ITransactionalRequest
{
    public Guid ApplicantId { get; set; }
    public Guid BootcampId { get; set; }
    public Guid ApplicationStateId { get; set; }

    public string[] Roles => [Admin, Write, ApplicationsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplications"];

    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, CreatedApplicationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        public CreateApplicationCommandHandler(IMapper mapper, IApplicationService applicationService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
        }

        public async Task<CreatedApplicationResponse> Handle(
            CreateApplicationCommand request,
            CancellationToken cancellationToken
        )
        {
            ApplicationEntity application = _mapper.Map<ApplicationEntity>(request);

            await _applicationService.AddAsync(application);

            CreatedApplicationResponse response = _mapper.Map<CreatedApplicationResponse>(application);
            return response;
        }
    }
}
