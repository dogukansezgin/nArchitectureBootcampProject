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

namespace Application.Features.Applications.Commands.Delete;

public class DeleteApplicationCommand : IRequest<DeletedApplicationResponse>, ISecuredRequest
//,ICacheRemoverRequest,
//ILoggableRequest,
//ITransactionalRequest
{
    public Guid Id { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [Admin, Write, ApplicationsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetApplications"];

    public class DeleteApplicationCommandHandler : IRequestHandler<DeleteApplicationCommand, DeletedApplicationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        public DeleteApplicationCommandHandler(IMapper mapper, IApplicationService applicationService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
        }

        public async Task<DeletedApplicationResponse> Handle(
            DeleteApplicationCommand request,
            CancellationToken cancellationToken
        )
        {
            ApplicationEntity? application = await _applicationService.GetAsync(
                predicate: a => a.Id == request.Id,
                cancellationToken: cancellationToken
            );

            //await _applicationService.DeleteAsync(application!, request.IsPermament);
            await _applicationService.DeleteAsync(application!, true);

            DeletedApplicationResponse response = _mapper.Map<DeletedApplicationResponse>(application);
            response.IsPermament = request.IsPermament;
            return response;
        }
    }
}
