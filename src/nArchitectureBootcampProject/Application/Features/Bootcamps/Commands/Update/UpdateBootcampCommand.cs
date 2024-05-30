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

namespace Application.Features.Bootcamps.Commands.Update;

public class UpdateBootcampCommand : IRequest<UpdatedBootcampResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid InstructorId { get; set; }
    public Guid BootcampStateId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBootcamps"];

    public class UpdateBootcampCommandHandler : IRequestHandler<UpdateBootcampCommand, UpdatedBootcampResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public UpdateBootcampCommandHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<UpdatedBootcampResponse> Handle(UpdateBootcampCommand request, CancellationToken cancellationToken)
        {
            Bootcamp? bootcamp = await _bootcampService.GetAsync(
                predicate: b => b.Id == request.Id,
                cancellationToken: cancellationToken
            );

            bootcamp = _mapper.Map(request, bootcamp);

            await _bootcampService.UpdateAsync(bootcamp!);

            UpdatedBootcampResponse response = _mapper.Map<UpdatedBootcampResponse>(bootcamp);
            return response;
        }
    }
}
