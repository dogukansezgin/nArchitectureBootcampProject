using Application.Features.Bootcamps.Commands.Create;
using Application.Features.Bootcamps.Commands.Delete;
using Application.Features.Bootcamps.Commands.Update;
using Application.Features.Bootcamps.Queries.GetAllList;
using Application.Features.Bootcamps.Queries.GetById;
using Application.Features.Bootcamps.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BootcampsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateBootcampCommand createBootcampCommand)
    {
        CreatedBootcampResponse response = await Mediator.Send(createBootcampCommand);

        return Created(uri: "", response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBootcampCommand updateBootcampCommand)
    {
        UpdatedBootcampResponse response = await Mediator.Send(updateBootcampCommand);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedBootcampResponse response = await Mediator.Send(new DeleteBootcampCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdBootcampResponse response = await Mediator.Send(new GetByIdBootcampQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListBootcampQuery getListBootcampQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListBootcampListItemDto> response = await Mediator.Send(getListBootcampQuery);
        return Ok(response);
    }
    [HttpGet("getUnfinished")]
    public async Task<IActionResult> GetListUnfinished([FromQuery] PageRequest pageRequest)
    {
        GetListUnfinishedBootcampQuery getListUnfinishedBootcampQuery = new() { PageRequest = pageRequest }; 
        GetListResponse<GetListBootcampListItemDto> response = await Mediator.Send(getListUnfinishedBootcampQuery);
        return Ok(response);
    }
}
