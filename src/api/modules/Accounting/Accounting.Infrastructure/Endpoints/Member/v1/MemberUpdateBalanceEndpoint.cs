using Accounting.Application.Members.UpdateBalance.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for updating a utility member's balance.
/// </summary>
public static class MemberUpdateBalanceEndpoint
{
    internal static RouteGroupBuilder MapMemberUpdateBalanceEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id}/balance", async (DefaultIdType id, UpdateUtilityMemberBalanceCommand command, ISender mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID in URL does not match ID in request body");

            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberUpdateBalanceEndpoint))
        .WithSummary("Update member balance")
        .WithDescription("Updates a member's current balance")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

