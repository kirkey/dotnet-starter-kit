using Accounting.Application.Members.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for creating a new member.
/// </summary>
public static class MemberCreateEndpoint
{
    internal static RouteGroupBuilder MapMemberCreateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (CreateMemberCommand command, ISender mediator) =>
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberCreateEndpoint))
        .WithSummary("Create member")
        .WithDescription("Creates a new member account")
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

