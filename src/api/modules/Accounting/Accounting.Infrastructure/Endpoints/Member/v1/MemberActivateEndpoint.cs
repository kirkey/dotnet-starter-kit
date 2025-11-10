using Accounting.Application.Members.Activate.v1;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for activating a member.
/// </summary>
public static class MemberActivateEndpoint
{
    internal static RouteGroupBuilder MapMemberActivateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/{id}/activate", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new ActivateMemberCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberActivateEndpoint))
        .WithSummary("Activate member")
        .WithDescription("Activates a member account")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        return group;
    }
}

