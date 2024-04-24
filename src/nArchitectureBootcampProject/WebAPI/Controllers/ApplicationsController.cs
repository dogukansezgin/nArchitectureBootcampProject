using Application.Features.Applications.Commands.Create;
using Application.Features.Applications.Commands.Delete;
using Application.Features.Applications.Commands.Update;
using Application.Features.Applications.Queries.AppliedBootcamps;
using Application.Features.Applications.Queries.CheckApplication;
using Application.Features.Applications.Queries.GetById;
using Application.Features.Applications.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationsController : BaseController
{
    [HttpPost("post")]
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

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedApplicationResponse response = await Mediator.Send(new DeleteApplicationCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdApplicationResponse response = await Mediator.Send(new GetByIdApplicationQuery { Id = id });
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListApplicationQuery getListApplicationQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListApplicationListItemDto> response = await Mediator.Send(getListApplicationQuery);
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
        AppliedBootcampsQuery appliedBootcampsQuery = new() { ApplicantId= applicantId, PageRequest = pageRequest };
        GetListResponse<AppliedBootcampsResponse> response = await Mediator.Send(appliedBootcampsQuery);
        return Ok(response);
    }
}
