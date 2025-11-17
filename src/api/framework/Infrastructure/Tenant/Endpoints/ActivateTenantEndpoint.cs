using FSH.Framework.Core.Tenant.Features.ActivateTenant;

namespace FSH.Framework.Infrastructure.Tenant.Endpoints;
public static class ActivateTenantEndpoint
{
    internal static RouteHandlerBuilder MapActivateTenantEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/activate", (ISender mediator, string id) => mediator.Send(new ActivateTenantCommand(id)))
                                .WithName(nameof(ActivateTenantEndpoint))
                                .WithSummary("activate tenant")
                                .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Tenants))
                                .WithDescription("activate tenant");
    }
}
