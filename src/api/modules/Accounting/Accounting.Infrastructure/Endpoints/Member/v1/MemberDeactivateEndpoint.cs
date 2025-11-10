using Accounting.Application.Members.Deactivate.v1;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for deactivating a member.
/// </summary>
public static class MemberDeactivateEndpoint
{
    internal static RouteGroupBuilder MapMemberDeactivateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/{id}/deactivate", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new DeactivateMemberCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberDeactivateEndpoint))
        .WithSummary("Deactivate member")
        .WithDescription("Deactivates a member account")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        return group;
    }
}

