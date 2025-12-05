using Accounting.Infrastructure.Endpoints.Billing.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Billing;

/// <summary>
/// Endpoint configuration for Billing module.
/// Provides comprehensive REST API endpoints for managing billing.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class BillingEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Billing endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/billing").WithTags("billing");

        group.MapCreateInvoiceFromConsumptionEndpoint();
    }
}
