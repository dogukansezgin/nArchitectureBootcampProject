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
using static Application.Features.Bootcamps.Constants.BootcampsOperationClaims;

namespace Application.Features.Bootcamps.Commands.Restore;

public class RestoreBootcampCommand : IRequest<RestoredBootcampResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBootcamps"];

    public class RestoreBootcampCommandHandler : IRequestHandler<RestoreBootcampCommand, RestoredBootcampResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public RestoreBootcampCommandHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<RestoredBootcampResponse> Handle(RestoreBootcampCommand request, CancellationToken cancellationToken)
        {
            Bootcamp? bootcamp = await _bootcampService.GetAsync(
                predicate: b => b.Id == request.Id && b.DeletedDate != null,
                cancellationToken: cancellationToken,
                withDeleted: true
            );

            await _bootcampService.RestoreAsync(bootcamp!);

            RestoredBootcampResponse response = _mapper.Map<RestoredBootcampResponse>(bootcamp);
            return response;
        }
    }
}
