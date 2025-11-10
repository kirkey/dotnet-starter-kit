using Accounting.Application.Members.Get.v1;

namespace Accounting.Infrastructure.Endpoints.Member.v1;

/// <summary>
/// Endpoint for retrieving a member by ID.
/// </summary>
public static class MemberGetEndpoint
{
    internal static RouteGroupBuilder MapMemberGetEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetMemberRequest(id);
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MemberGetEndpoint))
        .WithSummary("Get member")
        .WithDescription("Retrieves a member by ID")
        .RequirePermission("Permissions.Accounting.View")
        .MapToApiVersion(1);

        return group;
    }
}

