namespace FSH.Framework.Infrastructure.Identity.Roles.Endpoints;
public static class GetRolesEndpoint
{
    public static RouteHandlerBuilder MapGetRolesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (IRoleService roleService) =>
        {
            return await roleService.GetRolesAsync().ConfigureAwait(false);
        })
        .WithName(nameof(GetRolesEndpoint))
        .WithSummary("Get a list of all roles")
        .RequirePermission("Permissions.Roles.View")
        .WithDescription("Retrieve a list of all roles available in the system.");
    }
}
