using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using Sgd.Api.Models.Groups;
using Sgd.Application.Groups.Commands.AddGroup;
using Sgd.Application.Groups.Queries.Common;
using Sgd.Application.Groups.Queries.GetGroupById;
using Sgd.Application.Groups.Queries.GetMyGroups;
using Sgd.Application.Groups.Queries.SearchGroups;

namespace Sgd.Api.Controllers;

[Route("[controller]")]
[Authorize]
public class GroupController(ISender sender) : ApiController
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GroupResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGroupById([FromRoute] string id)
    {
        if (!ObjectId.TryParse(id, out var groupId))
        {
            return NotFound();
        }

        var query = new GetGroupByIdQuery(groupId);
        var result = await sender.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<GroupResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchGroups([FromQuery] string? name)
    {
        var query = new SearchGroupsQuery(name);
        var result = await sender.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpGet("my-groups")]
    [ProducesResponseType(typeof(IReadOnlyList<GroupResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyGroups()
    {
        var query = new GetMyGroupsQuery();
        var result = await sender.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddGroup([FromBody] AddGroupRequest request)
    {
        var command = new AddGroupCommand(
            request.Name,
            request.Description,
            request.IsOpen,
            request.ImageUrl
        );

        var result = await sender.Send(command);
        return result.Match(id => CreatedAtAction(nameof(GetGroupById), new { id }, null), Problem);
    }
}
