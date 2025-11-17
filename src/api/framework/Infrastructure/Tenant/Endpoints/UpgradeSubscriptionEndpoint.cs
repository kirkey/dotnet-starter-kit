using FSH.Framework.Core.Tenant.Features.UpgradeSubscription;

namespace FSH.Framework.Infrastructure.Tenant.Endpoints;

public static class UpgradeSubscriptionEndpoint
{
    internal static RouteHandlerBuilder MapUpgradeTenantSubscriptionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/upgrade", (UpgradeSubscriptionCommand command, ISender mediator) => mediator.Send(command))
                                .WithName(nameof(UpgradeSubscriptionEndpoint))
                                .WithSummary("upgrade tenant subscription")
                                .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Tenants))
                                .WithDescription("upgrade tenant subscription");
    }
}
