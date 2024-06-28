using Application.Features.Employees.Constants;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.Instructors;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Instructors.Constants.InstructorsOperationClaims;

namespace Application.Features.Instructors.Queries.GetBasicInfoById;

public class GetBasicInfoByIdInstructorQuery : IRequest<GetBasicInfoByIdInstructorResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User];

    public class GetByIdInstructorQueryHandler
        : IRequestHandler<GetBasicInfoByIdInstructorQuery, GetBasicInfoByIdInstructorResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorService _instructorService;

        public GetByIdInstructorQueryHandler(IMapper mapper, IInstructorService instructorService)
        {
            _mapper = mapper;
            _instructorService = instructorService;
        }

        public async Task<GetBasicInfoByIdInstructorResponse> Handle(
            GetBasicInfoByIdInstructorQuery request,
            CancellationToken cancellationToken
        )
        {
            Instructor? instructor = await _instructorService.GetByIdAsync(request.Id);

            GetBasicInfoByIdInstructorResponse response = _mapper.Map<GetBasicInfoByIdInstructorResponse>(instructor);
            return response;
        }
    }
}
