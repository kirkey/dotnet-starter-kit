using Accounting.Application.Members.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for deleting a member.
/// </summary>
public static class MemberDeleteEndpoint
{
    internal static RouteGroupBuilder MapMemberDeleteEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new DeleteMemberCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberDeleteEndpoint))
        .WithSummary("Delete member")
        .WithDescription("Deletes an inactive member with no balance")
        .RequirePermission("Permissions.Accounting.Delete")
        .MapToApiVersion(1);

        return group;
    }
}

