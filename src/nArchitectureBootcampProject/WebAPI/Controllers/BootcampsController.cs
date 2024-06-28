using Application.Features.Bootcamps.Commands.Create;
using Application.Features.Bootcamps.Commands.Delete;
using Application.Features.Bootcamps.Commands.DeleteRange;
using Application.Features.Bootcamps.Commands.Restore;
using Application.Features.Bootcamps.Commands.RestoreRange;
using Application.Features.Bootcamps.Commands.Update;
using Application.Features.Bootcamps.Queries.GetAllList;
using Application.Features.Bootcamps.Queries.GetBasicInfoList;
using Application.Features.Bootcamps.Queries.GetById;
using Application.Features.Bootcamps.Queries.GetByName;
using Application.Features.Bootcamps.Queries.GetList;
using Application.Features.Bootcamps.Queries.GetListByInstructor;
using Application.Features.Bootcamps.Queries.GetListDeleted;
using Application.Features.Bootcamps.Queries.GetListFinished;
using Application.Features.Bootcamps.Queries.GetListUnfinished;
using Application.Features.Bootcamps.Queries.SearchAll;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BootcampsController : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] CreateBootcampCommand createBootcampCommand)
    {
        CreatedBootcampResponse response = await Mediator.Send(createBootcampCommand);

        return Created(uri: "", response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateBootcampCommand updateBootcampCommand)
    {
        UpdatedBootcampResponse response = await Mediator.Send(updateBootcampCommand);

        return Ok(response);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteBootcampCommand deleteBootcampCommand)
    {
        DeletedBootcampResponse response = await Mediator.Send(deleteBootcampCommand);

        return Ok(response);
    }

    [HttpPost("deleteRange")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeBootcampCommand deleteRangeBootcampCommand)
    {
        DeletedRangeBootcampResponse response = await Mediator.Send(deleteRangeBootcampCommand);

        return Ok(response);
    }

    [HttpPost("restore")]
    public async Task<IActionResult> Restore([FromBody] RestoreBootcampCommand restoreBootcampCommand)
    {
        RestoredBootcampResponse response = await Mediator.Send(restoreBootcampCommand);

        return Ok(response);
    }

    [HttpPost("restoreRange")]
    public async Task<IActionResult> RestoreRange([FromBody] RestoreRangeBootcampCommand restoreRangeBootcampCommand)
    {
        RestoredRangeBootcampResponse response = await Mediator.Send(restoreRangeBootcampCommand);

        return Ok(response);
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdBootcampResponse response = await Mediator.Send(new GetByIdBootcampQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName([FromRoute] string name)
    {
        GetByIdBootcampResponse response = await Mediator.Send(new GetByNameBootcampQuery { Name = name });
        return Ok(response);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListBootcampQuery getListBootcampQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListBootcampListItemDto> response = await Mediator.Send(getListBootcampQuery);
        return Ok(response);
    }

    [HttpGet("getDeleted")]
    public async Task<IActionResult> GetListDeleted([FromQuery] PageRequest pageRequest)
    {
        GetListDeletedBootcampQuery getListDeletedBootcampQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListDeletedBootcampListItemDto> response = await Mediator.Send(getListDeletedBootcampQuery);
        return Ok(response);
    }

    [HttpGet("getByInstructor")]
    public async Task<IActionResult> GetListByInstructor([FromQuery] PageRequest pageRequest, [FromQuery] Guid instructorId)
    {
        GetListByInstructorBootcampQuery getListByInstructorBootcampQuery =
            new() { PageRequest = pageRequest, InstructorId = instructorId };
        GetListResponse<GetListByInstructorBootcampListItemDto> response = await Mediator.Send(getListByInstructorBootcampQuery);
        return Ok(response);
    }

    [HttpGet("getUnfinished")]
    public async Task<IActionResult> GetListUnfinished([FromQuery] PageRequest pageRequest)
    {
        GetListUnfinishedBootcampQuery getListUnfinishedBootcampQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListUnfinishedBootcampListItemDto> response = await Mediator.Send(getListUnfinishedBootcampQuery);
        return Ok(response);
    }

    [HttpGet("getFinished")]
    public async Task<IActionResult> GetListFinished([FromQuery] PageRequest pageRequest)
    {
        GetListFinishedBootcampQuery getListFinishedBootcampQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListFinishedBootcampListItemDto> response = await Mediator.Send(getListFinishedBootcampQuery);
        return Ok(response);
    }

    [HttpGet("searchAll")]
    public async Task<IActionResult> SearchAll([FromQuery] PageRequest pageRequest)
    {
        SearchAllBootcampQuery searchAllBootcampQuery = new() { PageRequest = pageRequest };
        GetListResponse<SearchAllBootcampListItemDto> response = await Mediator.Send(searchAllBootcampQuery);
        return Ok(response);
    }

    [HttpGet("getBasicInfo")]
    public async Task<IActionResult> GetBasicInfoList([FromQuery] PageRequest pageRequest)
    {
        GetBasicInfoListBootcampQuery getBasicInfoListBootcampQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetBasicInfoBootcampListItemDto> response = await Mediator.Send(getBasicInfoListBootcampQuery);
        return Ok(response);
    }
}
