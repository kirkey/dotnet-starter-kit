using Accounting.Application.Members.Get.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for retrieving a utility member by ID.
/// </summary>
public static class MemberGetEndpoint
{
    internal static RouteGroupBuilder MapMemberGetEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetUtilityMemberRequest(id);
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberGetEndpoint))
        .WithSummary("Get utility member")
        .WithDescription("Retrieves a utility member by ID")
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

