using Application.Features.Employees.Constants;
using Application.Services.Employees;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.Employees.Constants.EmployeesOperationClaims;

namespace Application.Features.Employees.Queries.GetListDeleted;

public class GetListDeletedEmployeeQuery : IRequest<GetListResponse<GetListDeletedEmployeeListItemDto>> /*, ISecuredRequest, ICachableRequest*/
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListEmployees({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetEmployees";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListEmployeeQueryHandler
        : IRequestHandler<GetListDeletedEmployeeQuery, GetListResponse<GetListDeletedEmployeeListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public GetListEmployeeQueryHandler(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<GetListResponse<GetListDeletedEmployeeListItemDto>> Handle(
            GetListDeletedEmployeeQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<Employee>? employees = await _employeeService.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken,
                withDeleted: true,
                predicate: x => x.DeletedDate != null
            );

            GetListResponse<GetListDeletedEmployeeListItemDto> response = _mapper.Map<
                GetListResponse<GetListDeletedEmployeeListItemDto>
            >(employees);
            return response;
        }
    }
}
