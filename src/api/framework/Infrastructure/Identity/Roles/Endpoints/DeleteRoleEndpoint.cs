namespace FSH.Framework.Infrastructure.Identity.Roles.Endpoints;

public static class DeleteRoleEndpoint
{
    public static RouteHandlerBuilder MapDeleteRoleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (string id, IRoleService roleService) =>
        {
            await roleService.DeleteRoleAsync(id).ConfigureAwait(false);
        })
        .WithName(nameof(DeleteRoleEndpoint))
        .WithSummary("Delete a role by ID")
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Roles))
        .WithDescription("Remove a role from the system by its ID.");
    }
}

