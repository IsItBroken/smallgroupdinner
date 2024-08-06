using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using Sgd.Api.Models.Groups;
using Sgd.Application.Groups.Commands.AcceptGroupJoinRequest;
using Sgd.Application.Groups.Commands.AddGroup;
using Sgd.Application.Groups.Commands.JoinGroup;
using Sgd.Application.Groups.Commands.LeaveGroup;
using Sgd.Application.Groups.Commands.UpdateGroup;
using Sgd.Application.Groups.Commands.UpdateGroupRole;
using Sgd.Application.Groups.Queries.Common;
using Sgd.Application.Groups.Queries.GetGroupById;
using Sgd.Application.Groups.Queries.GetMembers;
using Sgd.Application.Groups.Queries.GetMyGroups;
using Sgd.Application.Groups.Queries.SearchGroups;

namespace Sgd.Api.Controllers;

[Route("[controller]")]
[Authorize]
public class GroupController(ISender sender) : ApiController
{
    /// <summary>
    /// Test description
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
            request.Name.Trim(),
            request.Description.Trim(),
            request.IsOpen,
            request.ImageUrl?.Trim()
        );

        var result = await sender.Send(command);
        return result.Match(id => CreatedAtAction(nameof(GetGroupById), new { id }, null), Problem);
    }

    [HttpPut("{groupId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateGroup(
        [FromRoute] string groupId,
        [FromBody] UpdateGroupRequest request
    )
    {
        if (!ObjectId.TryParse(groupId, out var groupObjectId))
        {
            return NotFound();
        }

        var command = new UpdateGroupCommand(
            groupObjectId,
            request.Name.Trim(),
            request.Description.Trim(),
            request.ImageUrl?.Trim(),
            request.IsOpen
        );

        var result = await sender.Send(command);
        return result.Match(_ => Ok(), Problem);
    }

    [HttpPost("{groupId}/join")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> JoinGroup([FromRoute] string groupId)
    {
        if (!ObjectId.TryParse(groupId, out var groupObjectId))
        {
            return BadRequest("Invalid group id");
        }

        var command = new JoinGroupCommand(groupObjectId);
        var result = await sender.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpPost("{groupId}/leave")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> LeaveGroup([FromRoute] string groupId)
    {
        if (!ObjectId.TryParse(groupId, out var groupObjectId))
        {
            return BadRequest("Invalid group id");
        }

        var command = new LeaveGroupCommand(groupObjectId);
        var result = await sender.Send(command);
        return result.Match(_ => Ok(), Problem);
    }

    [HttpGet("{groupId}/members")]
    [ProducesResponseType(typeof(IReadOnlyList<GroupMemberResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMembers([FromRoute] string groupId)
    {
        if (!ObjectId.TryParse(groupId, out var groupObjectId))
        {
            return NotFound();
        }

        var query = new GetMembersQuery(groupObjectId);
        var result = await sender.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPost("{groupId}/decide-group-request")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DecideGroupRequest(
        [FromRoute] string groupId,
        [FromBody] DecideGroupRequest request
    )
    {
        if (!ObjectId.TryParse(groupId, out var groupObjectId))
        {
            return NotFound();
        }

        if (!ObjectId.TryParse(request.UserId, out var userObjectId))
        {
            return BadRequest("Invalid user id");
        }

        var command = new AcceptGroupJoinRequestCommand(
            groupObjectId,
            userObjectId,
            request.Accepted
        );

        var result = await sender.Send(command);
        return result.Match(_ => Ok(), Problem);
    }

    [HttpPost("{groupId}/update-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateGroupRole(
        [FromRoute] string groupId,
        [FromBody] UpdateGroupRoleRequest request
    )
    {
        if (!ObjectId.TryParse(groupId, out var groupObjectId))
        {
            return NotFound();
        }

        if (!ObjectId.TryParse(request.UserId, out var userObjectId))
        {
            return BadRequest("Invalid user id");
        }

        var command = new UpdateGroupRoleCommand(groupObjectId, userObjectId, request.Role);

        var result = await sender.Send(command);
        return result.Match(_ => Ok(), Problem);
    }
}
