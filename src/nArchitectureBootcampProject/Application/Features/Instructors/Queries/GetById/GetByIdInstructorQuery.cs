using Application.Features.Employees.Constants;
using Application.Features.Users.Constants;
using Application.Services.Instructors;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Instructors.Constants.InstructorsOperationClaims;

namespace Application.Features.Instructors.Queries.GetById;

public class GetByIdInstructorQuery : IRequest<GetByIdInstructorResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public class GetByIdInstructorQueryHandler : IRequestHandler<GetByIdInstructorQuery, GetByIdInstructorResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorService _instructorService;

        public GetByIdInstructorQueryHandler(IMapper mapper, IInstructorService instructorService)
        {
            _mapper = mapper;
            _instructorService = instructorService;
        }

        public async Task<GetByIdInstructorResponse> Handle(GetByIdInstructorQuery request, CancellationToken cancellationToken)
        {
            Instructor? instructor = await _instructorService.GetByIdAsync(request.Id);

            GetByIdInstructorResponse response = _mapper.Map<GetByIdInstructorResponse>(instructor);
            return response;
        }
    }
}
