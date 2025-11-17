using Asp.Versioning;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.OnlineUsers;

/// <summary>
/// Endpoint for retrieving currently online users.
/// </summary>
public static class GetOnlineUsersEndpoint
{
    internal static RouteHandlerBuilder MapGetOnlineUsersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/online-users", async (ISender mediator) =>
                {
                    var response = await mediator.Send(new GetOnlineUsersQuery()).ConfigureAwait(false);
                    return Results.Ok(response);
                })
                .WithName(nameof(GetOnlineUsersEndpoint))
                .WithSummary("get online users")
                .WithDescription("retrieves the list of currently online users")
                .Produces<GetOnlineUsersResponse>()
                .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Messaging))
                .MapToApiVersion(new ApiVersion(1, 0));
    }
}

