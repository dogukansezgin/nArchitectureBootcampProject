using Application.Features.Applications.Rules;
using Application.Services.Applications;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Features.Applications.Queries.CheckApplication;
public class CheckApplicationQuery : IRequest<CheckApplicationResponse>
{
    public Guid ApplicantId { get; set; }
    public Guid BootcampId { get; set; }

    public class CheckApplicationQueryHandler : IRequestHandler<CheckApplicationQuery, CheckApplicationResponse>
    {
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;
        private readonly ApplicationBusinessRules _applicationBusinessRules;

        public CheckApplicationQueryHandler(IApplicationService applicationService, IMapper mapper, ApplicationBusinessRules applicationBusinessRules)
        {
            _applicationService = applicationService;
            _mapper = mapper;
            _applicationBusinessRules = applicationBusinessRules;
        }

        public async Task<CheckApplicationResponse> Handle(CheckApplicationQuery request, CancellationToken cancellationToken)
        {
            ApplicationEntity? application = await _applicationService.GetAsync(predicate: x => x.ApplicantId == request.ApplicantId && x.BootcampId == request.BootcampId, include: x => x.Include(x => x.ApplicationState));

            await _applicationBusinessRules.ApplicationShouldExistWhenSelected(application);

            CheckApplicationResponse response = _mapper.Map<CheckApplicationResponse>(application);
            return response;
        }
    }
}
