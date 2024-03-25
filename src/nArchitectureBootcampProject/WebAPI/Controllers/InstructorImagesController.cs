using Application.Features.InstructorImages.Commands.Create;
using Application.Features.InstructorImages.Commands.Delete;
using Application.Features.InstructorImages.Commands.Update;
using Application.Features.InstructorImages.Queries.GetById;
using Application.Features.InstructorImages.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstructorImagesController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateInstructorImageCommand createInstructorImageCommand)
    {
        CreatedInstructorImageResponse response = await Mediator.Send(createInstructorImageCommand);

        return Created(uri: "", response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateInstructorImageCommand updateInstructorImageCommand)
    {
        UpdatedInstructorImageResponse response = await Mediator.Send(updateInstructorImageCommand);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedInstructorImageResponse response = await Mediator.Send(new DeleteInstructorImageCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdInstructorImageResponse response = await Mediator.Send(new GetByIdInstructorImageQuery { Id = id });
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListInstructorImageQuery getListInstructorImageQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListInstructorImageListItemDto> response = await Mediator.Send(getListInstructorImageQuery);
        return Ok(response);
    }
}
