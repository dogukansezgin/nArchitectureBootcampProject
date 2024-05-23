using Application.Features.Instructors.Commands.DeleteRange;
using Application.Features.Instructors.Commands.Restore;
using Application.Features.Instructors.Commands.RestoreRange;
using Application.Features.Instructors.Commands.Create;
using Application.Features.Instructors.Commands.Delete;
using Application.Features.Instructors.Commands.Update;
using Application.Features.Instructors.Queries.GetBasicInfoList;
using Application.Features.Instructors.Queries.GetById;
using Application.Features.Instructors.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Application.Features.Instructors.Queries.GetListDeleted;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstructorsController : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] CreateInstructorCommand createInstructorCommand)
    {
        CreatedInstructorResponse response = await Mediator.Send(createInstructorCommand);

        return Created(uri: "", response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateInstructorCommand updateInstructorCommand)
    {
        UpdatedInstructorResponse response = await Mediator.Send(updateInstructorCommand);

        return Ok(response);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteInstructorCommand deleteInstructorCommand)
    {
        DeletedInstructorResponse response = await Mediator.Send(deleteInstructorCommand);

        return Ok(response);
    }

    [HttpPost("deleteRange")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeInstructorCommand deleteRangeInstructorCommand)
    {
        DeletedRangeInstructorResponse response = await Mediator.Send(deleteRangeInstructorCommand);

        return Ok(response);
    }

    [HttpPost("restore")]
    public async Task<IActionResult> Restore([FromBody] RestoreInstructorCommand restoreInstructorCommand)
    {
        RestoredInstructorResponse response = await Mediator.Send(restoreInstructorCommand);

        return Ok(response);
    }

    [HttpPost("restoreRange")]
    public async Task<IActionResult> RestoreRange([FromBody] RestoreRangeInstructorCommand restoreRangeInstructorCommand)
    {
        RestoredRangeInstructorResponse response = await Mediator.Send(restoreRangeInstructorCommand);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdInstructorResponse response = await Mediator.Send(new GetByIdInstructorQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListInstructorQuery getListInstructorQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListInstructorListItemDto> response = await Mediator.Send(getListInstructorQuery);
        return Ok(response);
    }

    [HttpGet("getDeleted")]
    public async Task<IActionResult> GetListDeleted([FromQuery] PageRequest pageRequest)
    {
        GetListDeletedInstructorQuery getListInstructorQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListDeletedInstructorListItemDto> response = await Mediator.Send(getListInstructorQuery);
        return Ok(response);
    }

    [HttpGet("getBasicInfo")]
    public async Task<IActionResult> GetBasicInfoList([FromQuery] PageRequest pageRequest)
    {
        GetBasicInfoListInstructorQuery getBasicInfoListInstructorQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetBasicInfoInstructorListItemDto> response = await Mediator.Send(getBasicInfoListInstructorQuery);
        return Ok(response);
    }
}
