namespace FSH.Framework.Infrastructure.Identity.Users.Endpoints;
public static class GetUserRolesEndpoint
{
    internal static RouteHandlerBuilder MapGetUserRolesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}/roles", (string id, IUserService service) =>
        {
            return service.GetUserRolesAsync(id, CancellationToken.None);
        })
        .WithName(nameof(GetUserRolesEndpoint))
        .WithSummary("get user roles")
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Users))
        .WithDescription("get user roles");
    }
}
