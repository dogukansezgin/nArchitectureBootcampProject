using Application.Features.ApplicationStates.Commands.Create;
using Application.Features.ApplicationStates.Commands.Delete;
using Application.Features.ApplicationStates.Commands.DeleteRange;
using Application.Features.ApplicationStates.Commands.Restore;
using Application.Features.ApplicationStates.Commands.RestoreRange;
using Application.Features.ApplicationStates.Commands.Update;
using Application.Features.ApplicationStates.Queries.GetById;
using Application.Features.ApplicationStates.Queries.GetByName;
using Application.Features.ApplicationStates.Queries.GetList;
using Application.Features.ApplicationStates.Queries.GetListDeleted;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationStatesController : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] CreateApplicationStateCommand createApplicationStateCommand)
    {
        CreatedApplicationStateResponse response = await Mediator.Send(createApplicationStateCommand);

        return Created(uri: "", response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateApplicationStateCommand updateApplicationStateCommand)
    {
        UpdatedApplicationStateResponse response = await Mediator.Send(updateApplicationStateCommand);

        return Ok(response);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteApplicationStateCommand deleteApplicationStateCommand)
    {
        DeletedApplicationStateResponse response = await Mediator.Send(deleteApplicationStateCommand);

        return Ok(response);
    }

    [HttpPost("deleteRange")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeApplicationStateCommand deleteRangeApplicationStateCommand)
    {
        DeletedRangeApplicationStateResponse response = await Mediator.Send(deleteRangeApplicationStateCommand);

        return Ok(response);
    }

    [HttpPost("restore")]
    public async Task<IActionResult> Restore([FromBody] RestoreApplicationStateCommand restoreApplicationStateCommand)
    {
        RestoredApplicationStateResponse response = await Mediator.Send(restoreApplicationStateCommand);

        return Ok(response);
    }

    [HttpPost("restoreRange")]
    public async Task<IActionResult> RestoreRange(
        [FromBody] RestoreRangeApplicationStateCommand restoreRangeApplicationStateCommand
    )
    {
        RestoredRangeApplicationStateResponse response = await Mediator.Send(restoreRangeApplicationStateCommand);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdApplicationStateResponse response = await Mediator.Send(new GetByIdApplicationStateQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("getByName/{name}")]
    public async Task<IActionResult> GetByName([FromRoute] string name)
    {
        GetByNameApplicationStateResponse response = await Mediator.Send(new GetByNameApplicationStateQuery { Name = name });
        return Ok(response);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListApplicationStateQuery getListApplicationStateQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListApplicationStateListItemDto> response = await Mediator.Send(getListApplicationStateQuery);
        return Ok(response);
    }

    [HttpGet("getDeleted")]
    public async Task<IActionResult> GetListDeleted([FromQuery] PageRequest pageRequest)
    {
        GetListDeletedApplicationStateQuery getListDeletedApplicationStateQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListDeletedApplicationStateListItemDto> response = await Mediator.Send(
            getListDeletedApplicationStateQuery
        );
        return Ok(response);
    }
}
