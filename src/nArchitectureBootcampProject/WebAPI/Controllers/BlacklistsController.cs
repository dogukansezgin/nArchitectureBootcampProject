using Application.Features.Blacklists.Commands.DeleteRange;
using Application.Features.Blacklists.Commands.Restore;
using Application.Features.Blacklists.Commands.RestoreRange;
using Application.Features.Blacklists.Commands.Create;
using Application.Features.Blacklists.Commands.Delete;
using Application.Features.Blacklists.Commands.Update;
using Application.Features.Blacklists.Queries.GetById;
using Application.Features.Blacklists.Queries.GetList;
using Application.Features.Blacklists.Queries.GetListDeleted;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Application.Features.Blacklists.Queries.GetByApplicantId;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlacklistsController : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] CreateBlacklistCommand createBlacklistCommand)
    {
        CreatedBlacklistResponse response = await Mediator.Send(createBlacklistCommand);

        return Created(uri: "", response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateBlacklistCommand updateBlacklistCommand)
    {
        UpdatedBlacklistResponse response = await Mediator.Send(updateBlacklistCommand);

        return Ok(response);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteBlacklistCommand deleteBlacklistCommand)
    {
        DeletedBlacklistResponse response = await Mediator.Send(deleteBlacklistCommand);

        return Ok(response);
    }

    [HttpPost("deleteRange")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeBlacklistCommand deleteRangeBlacklistCommand)
    {
        DeletedRangeBlacklistResponse response = await Mediator.Send(deleteRangeBlacklistCommand);

        return Ok(response);
    }

    [HttpPost("restore")]
    public async Task<IActionResult> Restore([FromBody] RestoreBlacklistCommand restoreBlacklistCommand)
    {
        RestoredBlacklistResponse response = await Mediator.Send(restoreBlacklistCommand);

        return Ok(response);
    }

    [HttpPost("restoreRange")]
    public async Task<IActionResult> RestoreRange([FromBody] RestoreRangeBlacklistCommand restoreRangeBlacklistCommand)
    {
        RestoredRangeBlacklistResponse response = await Mediator.Send(restoreRangeBlacklistCommand);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdBlacklistResponse response = await Mediator.Send(new GetByIdBlacklistQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("applicantId/{id}")]
    public async Task<IActionResult> GetByApplicantId([FromRoute] Guid id)
    {
        GetByApplicantIdBlacklistResponse response = await Mediator.Send(new GetByApplicantIdBlacklistQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListBlacklistQuery getListBlacklistQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListBlacklistListItemDto> response = await Mediator.Send(getListBlacklistQuery);
        return Ok(response);
    }

    [HttpGet("getDeleted")]
    public async Task<IActionResult> GetListDeleted([FromQuery] PageRequest pageRequest)
    {
        GetListDeletedBlacklistQuery getListDeletedBlacklistQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListDeletedBlacklistListItemDto> response = await Mediator.Send(getListDeletedBlacklistQuery);
        return Ok(response);
    }
}
