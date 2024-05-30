using Application.Features.Employees.Constants;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.Instructors;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Instructors.Constants.InstructorsOperationClaims;

namespace Application.Features.Instructors.Commands.Restore;

public class RestoreInstructorCommand : IRequest<RestoredInstructorResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetInstructors"];

    public class RestoreInstructorCommandHandler : IRequestHandler<RestoreInstructorCommand, RestoredInstructorResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorService _instructorService;

        public RestoreInstructorCommandHandler(IMapper mapper, IInstructorService instructorService)
        {
            _mapper = mapper;
            _instructorService = instructorService;
        }

        public async Task<RestoredInstructorResponse> Handle(
            RestoreInstructorCommand request,
            CancellationToken cancellationToken
        )
        {
            Instructor? instructor = await _instructorService.GetAsync(
                predicate: b => b.Id == request.Id && b.DeletedDate != null,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _instructorService.RestoreAsync(instructor!);

            RestoredInstructorResponse response = _mapper.Map<RestoredInstructorResponse>(instructor);
            return response;
        }
    }
}
