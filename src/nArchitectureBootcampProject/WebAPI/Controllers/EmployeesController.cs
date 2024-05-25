using Application.Features.Employees.Commands.Create;
using Application.Features.Employees.Commands.Delete;
using Application.Features.Employees.Commands.DeleteRange;
using Application.Features.Employees.Commands.Restore;
using Application.Features.Employees.Commands.RestoreRange;
using Application.Features.Employees.Commands.Update;
using Application.Features.Employees.Queries.GetById;
using Application.Features.Employees.Queries.GetList;
using Application.Features.Employees.Queries.GetListDeleted;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] CreateEmployeeCommand createEmployeeCommand)
    {
        CreatedEmployeeResponse response = await Mediator.Send(createEmployeeCommand);

        return Created(uri: "", response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateEmployeeCommand updateEmployeeCommand)
    {
        UpdatedEmployeeResponse response = await Mediator.Send(updateEmployeeCommand);

        return Ok(response);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteEmployeeCommand deleteEmployeeCommand)
    {
        DeletedEmployeeResponse response = await Mediator.Send(deleteEmployeeCommand);

        return Ok(response);
    }

    [HttpPost("deleteRange")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeEmployeeCommand deleteRangeEmployeeCommand)
    {
        DeletedRangeEmployeeResponse response = await Mediator.Send(deleteRangeEmployeeCommand);

        return Ok(response);
    }

    [HttpPost("restore")]
    public async Task<IActionResult> Restore([FromBody] RestoreEmployeeCommand restoreEmployeeCommand)
    {
        RestoredEmployeeResponse response = await Mediator.Send(restoreEmployeeCommand);

        return Ok(response);
    }

    [HttpPost("restoreRange")]
    public async Task<IActionResult> RestoreRange([FromBody] RestoreRangeEmployeeCommand restoreRangeEmployeeCommand)
    {
        RestoredRangeEmployeeResponse response = await Mediator.Send(restoreRangeEmployeeCommand);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdEmployeeResponse response = await Mediator.Send(new GetByIdEmployeeQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListEmployeeQuery getListEmployeeQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListEmployeeListItemDto> response = await Mediator.Send(getListEmployeeQuery);
        return Ok(response);
    }

    [HttpGet("getDeleted")]
    public async Task<IActionResult> GetListDeleted([FromQuery] PageRequest pageRequest)
    {
        GetListDeletedEmployeeQuery getListDeletedEmployeeQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListDeletedEmployeeListItemDto> response = await Mediator.Send(getListDeletedEmployeeQuery);
        return Ok(response);
    }
}
