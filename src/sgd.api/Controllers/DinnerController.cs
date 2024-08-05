using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using Sgd.Api.Models.Dinners;
using Sgd.Application.Dinners.Commands.AddDinner;
using Sgd.Application.Dinners.Commands.AddHostToDinner;
using Sgd.Application.Dinners.Commands.CancelDinner;
using Sgd.Application.Dinners.Commands.CancelSignUpForDinner;
using Sgd.Application.Dinners.Commands.RemoveHostFromDinner;
using Sgd.Application.Dinners.Commands.SIgnUpForDinner;
using Sgd.Application.Dinners.Commands.UpdateDinner;
using Sgd.Application.Dinners.Queries.Common;
using Sgd.Application.Dinners.Queries.GetDinnerById;
using Sgd.Application.Dinners.Queries.SearchDinners;

namespace Sgd.Api.Controllers;

[Route("[controller]")]
[Authorize]
public class DinnerController(ISender sender) : ApiController
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DinnerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDinnerById([FromRoute] string id)
    {
        if (!ObjectId.TryParse(id, out var dinnerId))
        {
            return NotFound();
        }

        var query = new GetDinnerByIdQuery(dinnerId);
        var result = await sender.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDinner([FromBody] AddDinnerRequest request)
    {
        if (!ObjectId.TryParse(request.GroupId, out var groupId))
        {
            return BadRequest("Invalid group id");
        }

        var command = new AddDinnerCommand(
            request.Name,
            request.Date,
            request.Description,
            groupId,
            request.Capacity,
            request.SignUpMethod,
            request.RandomSelectionTime,
            request.ImageUrl
        );

        var result = await sender.Send(command);
        return result.Match(
            id => CreatedAtAction(nameof(GetDinnerById), new { id }, null),
            Problem
        );
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDinner(
        [FromRoute] string id,
        [FromBody] UpdateDinnerRequest request
    )
    {
        if (!ObjectId.TryParse(id, out var dinnerId))
        {
            return BadRequest("Invalid group id");
        }

        var command = new UpdateDinnerCommand(
            dinnerId,
            request.Name,
            request.Date,
            request.Description,
            request.Capacity,
            request.SignUpMethod,
            request.RandomSelectionTime,
            request.ImageUrl
        );

        var result = await sender.Send(command);
        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPost("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelDinner([FromRoute] string id)
    {
        if (!ObjectId.TryParse(id, out var dinnerId))
        {
            return BadRequest("Invalid group id");
        }

        var command = new CancelDinnerCommand(dinnerId);
        var result = await sender.Send(command);
        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPost("{id}/add-host")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddHostToDinner(
        [FromRoute] string id,
        [FromBody] AddHostRequest request
    )
    {
        if (!ObjectId.TryParse(id, out var dinnerId))
        {
            return BadRequest("Invalid group id");
        }

        if (!ObjectId.TryParse(request.UserId, out var userId))
        {
            return BadRequest("Invalid user id");
        }

        var command = new AddHostToDinnerCommand(dinnerId, userId);
        var result = await sender.Send(command);
        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPost("{id}/remove-host")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveHostFromDinner(
        [FromRoute] string id,
        [FromBody] RemoveHostRequest request
    )
    {
        if (!ObjectId.TryParse(id, out var dinnerId))
        {
            return BadRequest("Invalid group id");
        }

        if (!ObjectId.TryParse(request.UserId, out var userId))
        {
            return BadRequest("Invalid user id");
        }

        var command = new RemoveHostFromDinnerCommand(dinnerId, userId);
        var result = await sender.Send(command);
        return result.Match(_ => NoContent(), Problem);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<DinnerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchDinners([FromQuery] string? name)
    {
        var query = new SearchDinnersQuery(name);
        var result = await sender.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPost("{id}/sign-up")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SignUpForDinner([FromRoute] string id)
    {
        if (!ObjectId.TryParse(id, out var dinnerId))
        {
            return NotFound();
        }

        var command = new SignUpForDinnerCommand(dinnerId);
        var result = await sender.Send(command);
        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPost("{id}/cancel-sign-up")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSignUpForDinner([FromRoute] string id)
    {
        if (!ObjectId.TryParse(id, out var dinnerId))
        {
            return NotFound();
        }

        var command = new CancelSignUpForDinnerCommand(dinnerId);
        var result = await sender.Send(command);
        return result.Match(_ => NoContent(), Problem);
    }

    [HttpGet("my-dinners")]
    [ProducesResponseType(typeof(IReadOnlyList<DinnerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyDinners()
    {
        return Ok();
    }

    [HttpGet("feed")]
    [ProducesResponseType(typeof(IReadOnlyList<DinnerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDinnerFeed()
    {
        return Ok();
    }
}
