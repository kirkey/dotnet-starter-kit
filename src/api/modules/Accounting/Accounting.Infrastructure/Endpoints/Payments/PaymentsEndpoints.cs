using Accounting.Infrastructure.Endpoints.Payments.v1;

namespace Accounting.Infrastructure.Endpoints.Payments;

/// <summary>
/// Endpoint configuration for Payments module.
/// </summary>
public static class PaymentsEndpoints
{
    /// <summary>
    /// Maps all Payments endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapPaymentsEndpoints(this IEndpointRouteBuilder app)
    {
        var paymentsGroup = app.MapGroup("/payments")
            .WithTags("Payments")
            .WithDescription("Endpoints for managing payments");

        // Version 1 endpoints
        paymentsGroup.MapAllocatePaymentEndpoint();

        return app;
    }
}
