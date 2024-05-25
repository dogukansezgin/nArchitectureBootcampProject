using Application.Features.Applicants.Commands.Create;
using Application.Features.Applicants.Commands.Delete;
using Application.Features.Applicants.Commands.Update;
using Application.Features.Applicants.Commands.UpdateInfoFromAuth;
using Application.Features.Applicants.Queries.GetById;
using Application.Features.Applicants.Queries.GetList;
using Application.Features.Applicants.Queries.GetListDeleted;
using Application.Features.Applicants.Commands.DeleteRange;
using Application.Features.Applicants.Commands.Restore;
using Application.Features.Applicants.Commands.RestoreRange;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Application.Features.Applicants.Queries.GetBasicInfoList;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicantsController : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] CreateApplicantCommand createApplicantCommand)
    {
        CreatedApplicantResponse response = await Mediator.Send(createApplicantCommand);

        return Created(uri: "", response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateApplicantCommand updateApplicantCommand)
    {
        UpdatedApplicantResponse response = await Mediator.Send(updateApplicantCommand);

        return Ok(response);
    }

    [HttpPut("UpdateFromAuth")]
    public async Task<IActionResult> UpdateFromAuth(
        [FromBody] UpdateApplicantInfoFromAuthCommand updateApplicantInfoFromAuthCommand
    )
    {
        UpdatedApplicantInfoFromAuthResponse response = await Mediator.Send(updateApplicantInfoFromAuthCommand);

        return Ok(response);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteApplicantCommand deleteApplicantCommand)
    {
        DeletedApplicantResponse response = await Mediator.Send(deleteApplicantCommand);

        return Ok(response);
    }

    [HttpPost("deleteRange")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeApplicantCommand deleteRangeApplicantCommand)
    {
        DeletedRangeApplicantResponse response = await Mediator.Send(deleteRangeApplicantCommand);

        return Ok(response);
    }

    [HttpPost("restore")]
    public async Task<IActionResult> Restore([FromBody] RestoreApplicantCommand restoreApplicantCommand)
    {
        RestoredApplicantResponse response = await Mediator.Send(restoreApplicantCommand);

        return Ok(response);
    }

    [HttpPost("restoreRange")]
    public async Task<IActionResult> RestoreRange([FromBody] RestoreRangeApplicantCommand restoreRangeApplicantCommand)
    {
        RestoredRangeApplicantResponse response = await Mediator.Send(restoreRangeApplicantCommand);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdApplicantResponse response = await Mediator.Send(new GetByIdApplicantQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListApplicantQuery getListApplicantQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListApplicantListItemDto> response = await Mediator.Send(getListApplicantQuery);
        return Ok(response);
    }

    [HttpGet("getDeleted")]
    public async Task<IActionResult> GetListDeleted([FromQuery] PageRequest pageRequest)
    {
        GetListDeletedApplicantQuery getListDeletedApplicantQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListDeletedApplicantListItemDto> response = await Mediator.Send(getListDeletedApplicantQuery);
        return Ok(response);
    }

    [HttpGet("getBasicInfo")]
    public async Task<IActionResult> GetBasicInfoList([FromQuery] PageRequest pageRequest)
    {
        GetBasicInfoListApplicantQuery getBasicInfoListApplicantQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetBasicInfoApplicantListItemDto> response = await Mediator.Send(getBasicInfoListApplicantQuery);
        return Ok(response);
    }
}
