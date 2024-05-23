using Application.Features.Instructors.Constants;
using Application.Services.Instructors;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Instructors.Constants.InstructorsOperationClaims;

namespace Application.Features.Instructors.Commands.Delete;

public class DeleteInstructorCommand
    : IRequest<DeletedInstructorResponse>
    //,
    //    ISecuredRequest,
    //    ICacheRemoverRequest,
    //    ILoggableRequest,
    //    ITransactionalRequest
{
    public Guid Id { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [Admin, Write, InstructorsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetInstructors"];

    public class DeleteInstructorCommandHandler : IRequestHandler<DeleteInstructorCommand, DeletedInstructorResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorService _instructorService;

        public DeleteInstructorCommandHandler(IMapper mapper, IInstructorService instructorService)
        {
            _mapper = mapper;
            _instructorService = instructorService;
        }

        public async Task<DeletedInstructorResponse> Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {
            Instructor? instructor = await _instructorService.GetAsync(
                predicate: i => i.Id == request.Id,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            instructor = await _instructorService.DeleteAsync(instructor!, request.IsPermament);

            DeletedInstructorResponse response = _mapper.Map<DeletedInstructorResponse>(instructor);
            response.IsPermament = request.IsPermament;
            response.DeletedDate = request.IsPermament ? DateTime.UtcNow : response.DeletedDate;
            return response;
        }
    }
}
