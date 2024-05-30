using Application.Features.Applicants.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.Applications;
using AutoMapper;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Applications.Constants.ApplicationsOperationClaims;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Features.Applications.Queries.GetById;

public class GetByIdApplicationQuery : IRequest<GetByIdApplicationResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, ApplicantsOperationClaims.User];

    public class GetByIdApplicationQueryHandler : IRequestHandler<GetByIdApplicationQuery, GetByIdApplicationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        public GetByIdApplicationQueryHandler(IMapper mapper, IApplicationService applicationService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
        }

        public async Task<GetByIdApplicationResponse> Handle(GetByIdApplicationQuery request, CancellationToken cancellationToken)
        {
            ApplicationEntity? application = await _applicationService.GetByIdAsync(request.Id);

            GetByIdApplicationResponse response = _mapper.Map<GetByIdApplicationResponse>(application);
            return response;
        }
    }
}
