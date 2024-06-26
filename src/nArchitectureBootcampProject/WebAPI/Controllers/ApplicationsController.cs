using Application.Features.Applications.Commands.Create;
using Application.Features.Applications.Commands.Delete;
using Application.Features.Applications.Commands.DeleteRange;
using Application.Features.Applications.Commands.Restore;
using Application.Features.Applications.Commands.RestoreRange;
using Application.Features.Applications.Commands.Update;
using Application.Features.Applications.Commands.UpdateRange;
using Application.Features.Applications.Queries.AppliedBootcamps;
using Application.Features.Applications.Queries.CheckApplication;
using Application.Features.Applications.Queries.GetById;
using Application.Features.Applications.Queries.GetList;
using Application.Features.Applications.Queries.GetListByInstructor;
using Application.Features.Applications.Queries.GetListByInstructorByState;
using Application.Features.Applications.Queries.GetListByInstructorDeleted;
using Application.Features.Applications.Queries.GetListByJoin;
using Application.Features.Applications.Queries.GetListDeleted;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationsController : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] CreateApplicationCommand createApplicationCommand)
    {
        CreatedApplicationResponse response = await Mediator.Send(createApplicationCommand);

        return Created(uri: "", response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateApplicationCommand updateApplicationCommand)
    {
        UpdatedApplicationResponse response = await Mediator.Send(updateApplicationCommand);

        return Ok(response);
    }

    [HttpPut("updateRange")]
    public async Task<IActionResult> UpdateeRange([FromBody] UpdateRangeApplicationCommand updateRangeApplicationCommand)
    {
        UpdatedRangeApplicationResponse response = await Mediator.Send(updateRangeApplicationCommand);

        return Ok(response);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteApplicationCommand deleteApplicationCommand)
    {
        DeletedApplicationResponse response = await Mediator.Send(deleteApplicationCommand);

        return Ok(response);
    }

    [HttpPost("deleteRange")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeApplicationCommand deleteRangeApplicationCommand)
    {
        DeletedRangeApplicationResponse response = await Mediator.Send(deleteRangeApplicationCommand);

        return Ok(response);
    }

    [HttpPost("restore")]
    public async Task<IActionResult> Restore([FromBody] RestoreApplicationCommand restoreApplicationCommand)
    {
        RestoredApplicationResponse response = await Mediator.Send(restoreApplicationCommand);

        return Ok(response);
    }

    [HttpPost("restoreRange")]
    public async Task<IActionResult> RestoreRange([FromBody] RestoreRangeApplicationCommand restoreRangeApplicationCommand)
    {
        RestoredRangeApplicationResponse response = await Mediator.Send(restoreRangeApplicationCommand);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdApplicationResponse response = await Mediator.Send(new GetByIdApplicationQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListApplicationQuery getListApplicationQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListApplicationListItemDto> response = await Mediator.Send(getListApplicationQuery);
        return Ok(response);
    }

    [HttpGet("getDeleted")]
    public async Task<IActionResult> GetListDeleted([FromQuery] PageRequest pageRequest)
    {
        GetListDeletedApplicationQuery getListDeletedApplicationQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListDeletedApplicationListItemDto> response = await Mediator.Send(getListDeletedApplicationQuery);
        return Ok(response);
    }

    [HttpGet("getByInstructor")]
    public async Task<IActionResult> GetListByInstructor([FromQuery] PageRequest pageRequest, [FromQuery] Guid instructorId)
    {
        GetListByInstructorApplicationQuery getListByInstructorApplicationQuery =
            new() { PageRequest = pageRequest, InstructorId = instructorId };
        GetListResponse<GetListByInstructorApplicationListItemDto> response = await Mediator.Send(
            getListByInstructorApplicationQuery
        );
        return Ok(response);
    }

    [HttpGet("getByInstructorDeleted")]
    public async Task<IActionResult> GetListByInstructorDeleted(
        [FromQuery] PageRequest pageRequest,
        [FromQuery] Guid instructorId
    )
    {
        GetListByInstructorDeletedApplicationQuery getListByInstructorDeletedApplicationQuery =
            new() { PageRequest = pageRequest, InstructorId = instructorId };
        GetListResponse<GetListByInstructorDeletedApplicationListItemDto> response = await Mediator.Send(
            getListByInstructorDeletedApplicationQuery
        );
        return Ok(response);
    }

    [HttpGet("getByInstructorByState")]
    public async Task<IActionResult> GetListByInstructorByState(
        [FromQuery] PageRequest pageRequest,
        [FromQuery] Guid instructorId
    )
    {
        GetListByInstructorByStateApplicationQuery getListByInstructorByStateApplicationQuery =
            new() { PageRequest = pageRequest, InstructorId = instructorId };
        GetListResponse<GetListByInstructorByStateApplicationListItemDto> response = await Mediator.Send(
            getListByInstructorByStateApplicationQuery
        );
        return Ok(response);
    }

    [HttpGet("getByJoin")]
    public async Task<IActionResult> GetListByUserName([FromQuery] PageRequest pageRequest)
    {
        GetListByJoinApplicationQuery getListByUserNameApplicationQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListByJoinApplicationListItemDto> response = await Mediator.Send(getListByUserNameApplicationQuery);
        return Ok(response);
    }

    [HttpGet("checkApplication")]
    public async Task<IActionResult> Check([FromQuery] Guid applicantId, [FromQuery] Guid bootcampId)
    {
        CheckApplicationQuery checkApplicationQuery = new() { ApplicantId = applicantId, BootcampId = bootcampId };
        CheckApplicationResponse response = await Mediator.Send(checkApplicationQuery);
        return Ok(response);
    }

    [HttpGet("appliedBootcamps")]
    public async Task<IActionResult> AppliedBootcamps([FromQuery] PageRequest pageRequest, [FromQuery] Guid applicantId)
    {
        AppliedBootcampsQuery appliedBootcampsQuery = new() { ApplicantId = applicantId, PageRequest = pageRequest };
        GetListResponse<AppliedBootcampsResponse> response = await Mediator.Send(appliedBootcampsQuery);
        return Ok(response);
    }
}
