using Application.Features.Instructors.Constants;
using Application.Services.Instructors;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Instructors.Constants.InstructorsOperationClaims;

namespace Application.Features.Instructors.Commands.DeleteRange;

public class DeleteRangeInstructorCommand : IRequest<DeletedRangeInstructorResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [Admin, Write, InstructorsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetInstructors" };

    public class DeleteRangeInstructorCommandHandler
        : IRequestHandler<DeleteRangeInstructorCommand, DeletedRangeInstructorResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorService _instructorService;

        public DeleteRangeInstructorCommandHandler(IMapper mapper, IInstructorService instructorService)
        {
            _mapper = mapper;
            _instructorService = instructorService;
        }

        public async Task<DeletedRangeInstructorResponse> Handle(
            DeleteRangeInstructorCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Instructor>? instructors = await _instructorService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _instructorService.DeleteRangeAsync(instructors.Items, request.IsPermament);

            DeletedRangeInstructorResponse response = new DeletedRangeInstructorResponse
            {
                Ids = instructors.Items.Select(b => b.Id).ToList(),
                DeletedDate = DateTime.UtcNow,
                IsPermament = request.IsPermament
            };

            return response;
        }
    }
}
