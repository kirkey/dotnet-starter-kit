using Accounting.Application.Members.Activate.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for activating a utility member.
/// </summary>
public static class MemberActivateEndpoint
{
    internal static RouteGroupBuilder MapMemberActivateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/{id}/activate", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new ActivateUtilityMemberCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberActivateEndpoint))
        .WithSummary("Activate utility member")
        .WithDescription("Activates a utility member account")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

