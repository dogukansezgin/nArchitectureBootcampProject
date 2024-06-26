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

namespace Application.Features.Bootcamps.Commands.Create;

public class CreateBootcampCommand : IRequest<CreatedBootcampResponse>, ISecuredRequest
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public string Name { get; set; }
    public Guid InstructorId { get; set; }
    public Guid BootcampStateId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetBootcamps"];

    public class CreateBootcampCommandHandler : IRequestHandler<CreateBootcampCommand, CreatedBootcampResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public CreateBootcampCommandHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<CreatedBootcampResponse> Handle(CreateBootcampCommand request, CancellationToken cancellationToken)
        {
            Bootcamp bootcamp = _mapper.Map<Bootcamp>(request);
            Console.WriteLine(bootcamp.CreatedDate);
            await _bootcampService.AddAsync(bootcamp);
            Console.WriteLine(bootcamp.CreatedDate);
            bootcamp.CreatedDate = DateTime.Now;
            Console.WriteLine(bootcamp.CreatedDate);

            CreatedBootcampResponse response = _mapper.Map<CreatedBootcampResponse>(bootcamp);
            return response;
        }
    }
}
