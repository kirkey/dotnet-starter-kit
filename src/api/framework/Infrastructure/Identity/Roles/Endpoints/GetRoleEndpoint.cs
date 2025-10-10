namespace FSH.Framework.Infrastructure.Identity.Roles.Endpoints;

public static class GetRoleByIdEndpoint
{
    public static RouteHandlerBuilder MapGetRoleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (string id, IRoleService roleService) =>
        {
            return await roleService.GetRoleAsync(id).ConfigureAwait(false);
        })
        .WithName(nameof(GetRoleByIdEndpoint))
        .WithSummary("Get role details by ID")
        .RequirePermission("Permissions.Roles.View")
        .WithDescription("Retrieve the details of a role by its ID.");
    }
}

