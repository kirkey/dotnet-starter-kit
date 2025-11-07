using Accounting.Infrastructure.Endpoints.PaymentAllocations.v1;

namespace Accounting.Infrastructure.Endpoints.PaymentAllocations;

/// <summary>
/// Endpoint configuration for Payment Allocations module.
/// </summary>
public static class PaymentAllocationsEndpoints
{
    /// <summary>
    /// Maps all Payment Allocations endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapPaymentAllocationsEndpoints(this IEndpointRouteBuilder app)
    {
        var paymentAllocationsGroup = app.MapGroup("/payment-allocations")
            .WithTags("Payment-Allocations")
            .WithDescription("Endpoints for managing payment allocations");

        // Version 1 endpoints - CRUD operations
        paymentAllocationsGroup.MapPaymentAllocationCreateEndpoint();
        paymentAllocationsGroup.MapPaymentAllocationGetEndpoint();
        paymentAllocationsGroup.MapPaymentAllocationUpdateEndpoint();
        paymentAllocationsGroup.MapPaymentAllocationDeleteEndpoint();
        paymentAllocationsGroup.MapPaymentAllocationSearchEndpoint();

        return app;
    }
}
