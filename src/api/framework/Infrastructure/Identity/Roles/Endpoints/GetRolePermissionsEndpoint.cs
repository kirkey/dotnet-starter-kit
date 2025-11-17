namespace FSH.Framework.Infrastructure.Identity.Roles.Endpoints;
public static class GetRolePermissionsEndpoint
{
    public static RouteHandlerBuilder MapGetRolePermissionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}/permissions", async (string id, IRoleService roleService, CancellationToken cancellationToken) =>
        {
            return await roleService.GetWithPermissionsAsync(id, cancellationToken).ConfigureAwait(false);
        })
        .WithName(nameof(GetRolePermissionsEndpoint))
        .WithSummary("get role permissions")
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Roles))
        .WithDescription("get role permissions");
    }
}
