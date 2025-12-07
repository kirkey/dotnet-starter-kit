using Accounting.Application.Members.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for creating a new utility member.
/// </summary>
public static class MemberCreateEndpoint
{
    internal static RouteGroupBuilder MapMemberCreateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (CreateUtilityMemberCommand command, ISender mediator) =>
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberCreateEndpoint))
        .WithSummary("Create utility member")
        .WithDescription("Creates a new utility member account")
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

