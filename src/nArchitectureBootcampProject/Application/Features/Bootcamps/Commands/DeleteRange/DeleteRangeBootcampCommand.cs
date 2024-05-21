using Application.Features.Bootcamps.Constants;
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

namespace Application.Features.Bootcamps.Commands.DeleteRange;

public class DeleteRangeBootcampCommand : IRequest<DeletedRangeBootcampResponse>
//,
//    ISecuredRequest,
//    ICacheRemoverRequest,
//    ILoggableRequest,
//    ITransactionalRequest
{
    public List<Guid> Ids { get; set; }
    public bool IsPermament { get; set; }

    public string[] Roles => [Admin, Write, BootcampsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => new[] { "GetBootcamps" };

    public class DeleteRangeBootcampCommandHandler : IRequestHandler<DeleteRangeBootcampCommand, DeletedRangeBootcampResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public DeleteRangeBootcampCommandHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<DeletedRangeBootcampResponse> Handle(
            DeleteRangeBootcampCommand request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Bootcamp>? bootcamps = await _bootcampService.GetListAsync(
                size: request.Ids.Count,
                predicate: b => request.Ids.Contains(b.Id),
                cancellationToken: cancellationToken,
                withDeleted: true
            );
            Console.WriteLine(bootcamps.Items);
            await _bootcampService.DeleteRangeAsync(bootcamps.Items, request.IsPermament);

            DeletedRangeBootcampResponse response = new DeletedRangeBootcampResponse
            {
                Ids = bootcamps.Items.Select(b => b.Id).ToList(),
                DeletedDate = DateTime.UtcNow,
                IsPermament = request.IsPermament
            };

            return response;
        }
    }
}
