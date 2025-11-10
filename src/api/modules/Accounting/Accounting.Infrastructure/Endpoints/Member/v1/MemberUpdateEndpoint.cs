using Accounting.Application.Members.Update.v1;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for updating a member.
/// </summary>
public static class MemberUpdateEndpoint
{
    internal static RouteGroupBuilder MapMemberUpdateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id}", async (DefaultIdType id, UpdateMemberCommand command, ISender mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID in URL does not match ID in request body");

            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberUpdateEndpoint))
        .WithSummary("Update member")
        .WithDescription("Updates a member account")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        return group;
    }
}

