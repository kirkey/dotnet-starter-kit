using Accounting.Infrastructure.Endpoints.PaymentAllocations.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.PaymentAllocations;

/// <summary>
/// Endpoint configuration for PaymentAllocations module.
/// Provides comprehensive REST API endpoints for managing payment-allocations.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class PaymentAllocationsEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all PaymentAllocations endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/payment-allocations").WithTags("payment-allocation");

        group.MapPaymentAllocationCreateEndpoint();
        group.MapPaymentAllocationDeleteEndpoint();
        group.MapPaymentAllocationGetEndpoint();
        group.MapPaymentAllocationSearchEndpoint();
        group.MapPaymentAllocationUpdateEndpoint();
    }
}
