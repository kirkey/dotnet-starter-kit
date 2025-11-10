using Accounting.Application.Members.Search.v1;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for searching members.
/// </summary>
public static class MemberSearchEndpoint
{
    internal static RouteGroupBuilder MapMemberSearchEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/search", async (SearchMembersRequest request, ISender mediator) =>
        {
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberSearchEndpoint))
        .WithSummary("Search members")
        .WithDescription("Search members with filters and pagination")
        .RequirePermission("Permissions.Accounting.View")
        .MapToApiVersion(1);

        return group;
    }
}

