using Application.Features.Applicants.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.Bootcamps;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Bootcamps.Constants.BootcampsOperationClaims;

namespace Application.Features.Bootcamps.Queries.GetById;

public class GetByIdBootcampQuery : IRequest<GetByIdBootcampResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User, ApplicantsOperationClaims.User];

    public class GetByIdBootcampQueryHandler : IRequestHandler<GetByIdBootcampQuery, GetByIdBootcampResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public GetByIdBootcampQueryHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<GetByIdBootcampResponse> Handle(GetByIdBootcampQuery request, CancellationToken cancellationToken)
        {
            Bootcamp? bootcamp = await _bootcampService.GetByIdAsync(request.Id);

            GetByIdBootcampResponse response = _mapper.Map<GetByIdBootcampResponse>(bootcamp);
            return response;
        }
    }
}
