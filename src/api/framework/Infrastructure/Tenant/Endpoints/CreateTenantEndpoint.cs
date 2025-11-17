using FSH.Framework.Core.Tenant.Features.CreateTenant;

namespace FSH.Framework.Infrastructure.Tenant.Endpoints;
public static class CreateTenantEndpoint
{
    internal static RouteHandlerBuilder MapRegisterTenantEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", (CreateTenantCommand request, ISender mediator) => mediator.Send(request))
                                .WithName(nameof(CreateTenantEndpoint))
                                .WithSummary("creates a tenant")
                                .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Tenants))
                                .WithDescription("creates a tenant");
    }
}
