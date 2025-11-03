using Accounting.Infrastructure.Endpoints.Billing.v1;

namespace Accounting.Infrastructure.Endpoints.Billing;

/// <summary>
/// Endpoint configuration for Billing module.
/// </summary>
public static class BillingEndpoints
{
    /// <summary>
    /// Maps all Billing endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapBillingEndpoints(this IEndpointRouteBuilder app)
    {
        var billingGroup = app.MapGroup("/billing")
            .WithDescription("Endpoints for managing billing operations");

        // Version 1 endpoints
        billingGroup.MapCreateInvoiceFromConsumptionEndpoint();

        return app;
    }
}
