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

namespace Application.Features.Instructors.Commands.RestoreRange;

public class RestoreRangeInstructorCommand : IRequest<RestoredRangeInstructorResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }

    public string[] Roles => [Admin, Write];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetInstructors" };

    public class RestoreRangeInstructorCommandHandler
        : IRequestHandler<RestoreRangeInstructorCommand, RestoredRangeInstructorResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorService _instructorService;

        public RestoreRangeInstructorCommandHandler(IMapper mapper, IInstructorService instructorService)
        {
            _mapper = mapper;
            _instructorService = instructorService;
        }

        public async Task<RestoredRangeInstructorResponse> Handle(
            RestoreRangeInstructorCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Instructor>? instructors = await _instructorService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _instructorService.RestoreRangeAsync(instructors.Items);

            RestoredRangeInstructorResponse response = new RestoredRangeInstructorResponse
            {
                Ids = instructors.Items.Select(b => b.Id).ToList()
            };

            return response;
        }
    }
}
