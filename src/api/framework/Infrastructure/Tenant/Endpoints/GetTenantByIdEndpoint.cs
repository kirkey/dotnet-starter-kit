using FSH.Framework.Core.Tenant.Features.GetTenantById;

namespace FSH.Framework.Infrastructure.Tenant.Endpoints;
public static class GetTenantByIdEndpoint
{
    internal static RouteHandlerBuilder MapGetTenantByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}", (ISender mediator, string id) => mediator.Send(new GetTenantByIdQuery(id)))
                                .WithName(nameof(GetTenantByIdEndpoint))
                                .WithSummary("get tenant by id")
                                .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Tenants))
                                .WithDescription("get tenant by id");
    }
}
