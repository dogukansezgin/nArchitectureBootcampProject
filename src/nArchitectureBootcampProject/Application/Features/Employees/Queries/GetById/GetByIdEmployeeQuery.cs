using Application.Features.Employees.Constants;
using Application.Features.Employees.Rules;
using Application.Services.Employees;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Employees.Constants.EmployeesOperationClaims;

namespace Application.Features.Employees.Queries.GetById;

public class GetByIdEmployeeQuery : IRequest<GetByIdEmployeeResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdEmployeeQueryHandler : IRequestHandler<GetByIdEmployeeQuery, GetByIdEmployeeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public GetByIdEmployeeQueryHandler(
            IMapper mapper
, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<GetByIdEmployeeResponse> Handle(GetByIdEmployeeQuery request, CancellationToken cancellationToken)
        {
            Employee? employee = await _employeeService.GetByIdAsync(request.Id);

            GetByIdEmployeeResponse response = _mapper.Map<GetByIdEmployeeResponse>(employee);
            return response;
        }
    }
}
