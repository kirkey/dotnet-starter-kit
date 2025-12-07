using Accounting.Application.Members.Responses;
using Accounting.Application.Members.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for searching utility members.
/// </summary>
public static class MemberSearchEndpoint
{
    internal static RouteGroupBuilder MapMemberSearchEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/search", async (SearchUtilityMembersRequest request, ISender mediator) =>
        {
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberSearchEndpoint))
        .WithSummary("Search utility members")
        .WithDescription("Search utility members with filters and pagination")
        .Produces<PagedList<UtilityMemberResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

