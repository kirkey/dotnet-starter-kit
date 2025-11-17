namespace FSH.Framework.Infrastructure.Identity.Audit.Endpoints;

public static class GetUserAuditTrailEndpoint
{
    internal static RouteHandlerBuilder MapGetUserAuditTrailEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}/audit-trails", (DefaultIdType id, IAuditService service) =>
        {
            return service.GetUserTrailsAsync(id);
        })
        .WithName(nameof(GetUserAuditTrailEndpoint))
        .WithSummary("Get user's audit trail details")
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.AuditTrails))
        .WithDescription("Get user's audit trail details.");
    }
}
