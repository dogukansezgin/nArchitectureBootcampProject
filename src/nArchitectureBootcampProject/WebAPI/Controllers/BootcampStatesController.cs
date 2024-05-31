using Application.Features.BootcampStates.Commands.Create;
using Application.Features.BootcampStates.Commands.Delete;
using Application.Features.BootcampStates.Commands.DeleteRange;
using Application.Features.BootcampStates.Commands.Restore;
using Application.Features.BootcampStates.Commands.RestoreRange;
using Application.Features.BootcampStates.Commands.Update;
using Application.Features.BootcampStates.Queries.GetById;
using Application.Features.BootcampStates.Queries.GetByName;
using Application.Features.BootcampStates.Queries.GetList;
using Application.Features.BootcampStates.Queries.GetListDeleted;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BootcampStatesController : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] CreateBootcampStateCommand createBootcampStateCommand)
    {
        CreatedBootcampStateResponse response = await Mediator.Send(createBootcampStateCommand);

        return Created(uri: "", response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateBootcampStateCommand updateBootcampStateCommand)
    {
        UpdatedBootcampStateResponse response = await Mediator.Send(updateBootcampStateCommand);

        return Ok(response);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteBootcampStateCommand deleteBootcampStateCommand)
    {
        DeletedBootcampStateResponse response = await Mediator.Send(deleteBootcampStateCommand);

        return Ok(response);
    }

    [HttpPost("deleteRange")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeBootcampStateCommand deleteRangeBootcampStateCommand)
    {
        DeletedRangeBootcampStateResponse response = await Mediator.Send(deleteRangeBootcampStateCommand);

        return Ok(response);
    }

    [HttpPost("restore")]
    public async Task<IActionResult> Restore([FromBody] RestoreBootcampStateCommand restoreBootcampStateCommand)
    {
        RestoredBootcampStateResponse response = await Mediator.Send(restoreBootcampStateCommand);

        return Ok(response);
    }

    [HttpPost("restoreRange")]
    public async Task<IActionResult> RestoreRange([FromBody] RestoreRangeBootcampStateCommand restoreRangeBootcampStateCommand)
    {
        RestoredRangeBootcampStateResponse response = await Mediator.Send(restoreRangeBootcampStateCommand);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdBootcampStateResponse response = await Mediator.Send(new GetByIdBootcampStateQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("getByName/{name}")]
    public async Task<IActionResult> GetByName([FromRoute] String name)
    {
        GetByNameBootcampStateResponse response = await Mediator.Send(new GetByNameBootcampStateQuery { Name = name });
        return Ok(response);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListBootcampStateQuery getListBootcampStateQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListBootcampStateListItemDto> response = await Mediator.Send(getListBootcampStateQuery);
        return Ok(response);
    }

    [HttpGet("getDeleted")]
    public async Task<IActionResult> GetListDeleted([FromQuery] PageRequest pageRequest)
    {
        GetListDeletedBootcampStateQuery getListDeletedBootcampStateQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListDeletedBootcampStateListItemDto> response = await Mediator.Send(getListDeletedBootcampStateQuery);
        return Ok(response);
    }
}
