using Application.Features.Bootcamps.Constants;
using Application.Features.Employees.Constants;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.Bootcamps;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Bootcamps.Constants.BootcampsOperationClaims;

namespace Application.Features.Bootcamps.Commands.RestoreRange;

public class RestoreRangeBootcampCommand : IRequest<RestoredRangeBootcampResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetBootcamps" };

    public class RestoreRangeBootcampCommandHandler : IRequestHandler<RestoreRangeBootcampCommand, RestoredRangeBootcampResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public RestoreRangeBootcampCommandHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<RestoredRangeBootcampResponse> Handle(
            RestoreRangeBootcampCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Bootcamp>? bootcamps = await _bootcampService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _bootcampService.RestoreRangeAsync(bootcamps.Items);

            RestoredRangeBootcampResponse response = new RestoredRangeBootcampResponse
            {
                Ids = bootcamps.Items.Select(b => b.Id).ToList()
            };

            return response;
        }
    }
}
