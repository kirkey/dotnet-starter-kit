using Accounting.Application.Members.UpdateBalance.v1;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for updating a member's balance.
/// </summary>
public static class MemberUpdateBalanceEndpoint
{
    internal static RouteGroupBuilder MapMemberUpdateBalanceEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id}/balance", async (DefaultIdType id, UpdateMemberBalanceCommand command, ISender mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID in URL does not match ID in request body");

            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberUpdateBalanceEndpoint))
        .WithSummary("Update member balance")
        .WithDescription("Updates a member's current balance")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        return group;
    }
}

