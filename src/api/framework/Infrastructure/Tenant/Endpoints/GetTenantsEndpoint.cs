using FSH.Framework.Core.Tenant.Features.GetTenants;

namespace FSH.Framework.Infrastructure.Tenant.Endpoints;
public static class GetTenantsEndpoint
{
    internal static RouteHandlerBuilder MapGetTenantsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", (ISender mediator) => mediator.Send(new GetTenantsQuery()))
                                .WithName(nameof(GetTenantsEndpoint))
                                .WithSummary("get tenants")
                                .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Tenants))
                                .WithDescription("get tenants");
    }
}
