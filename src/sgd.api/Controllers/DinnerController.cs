using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using Sgd.Api.Models.Dinners;
using Sgd.Application.Dinners.Commands.AddDinner;
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
        var command = new AddDinnerCommand(
            request.Name,
            request.Date,
            request.Description,
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

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<DinnerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchDinners([FromQuery] string? name)
    {
        var query = new SearchDinnersQuery(name);
        var result = await sender.Send(query);
        return result.Match(Ok, Problem);
    }
}
