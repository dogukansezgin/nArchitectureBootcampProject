using Application.Features.Bootcamps.Queries.GetById;
using Application.Services.Bootcamps;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Bootcamps.Constants.BootcampsOperationClaims;

namespace Application.Features.Bootcamps.Queries.GetByName;

public class GetByNameBootcampQuery : IRequest<GetByIdBootcampResponse> /*, ISecuredRequest*/
{
    public string Name { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByNameBootcampQueryHandler : IRequestHandler<GetByNameBootcampQuery, GetByIdBootcampResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBootcampService _bootcampService;

        public GetByNameBootcampQueryHandler(IMapper mapper, IBootcampService bootcampService)
        {
            _mapper = mapper;
            _bootcampService = bootcampService;
        }

        public async Task<GetByIdBootcampResponse> Handle(GetByNameBootcampQuery request, CancellationToken cancellationToken)
        {
            Bootcamp? bootcamp = await _bootcampService.GetByNameAsync(request.Name);

            GetByIdBootcampResponse response = _mapper.Map<GetByIdBootcampResponse>(bootcamp);
            return response;
        }
    }
}
