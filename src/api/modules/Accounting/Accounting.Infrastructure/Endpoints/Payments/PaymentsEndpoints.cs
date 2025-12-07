using Accounting.Infrastructure.Endpoints.Payments.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Payments;

/// <summary>
/// Endpoint configuration for Payments module.
/// Provides comprehensive REST API endpoints for managing payments.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class PaymentsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Payments endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/payments").WithTags("payment");

        group.MapAllocatePaymentEndpoint();
        group.MapPaymentCreateEndpoint();
        group.MapPaymentDeleteEndpoint();
        group.MapPaymentGetEndpoint();
        group.MapPaymentRefundEndpoint();
        group.MapPaymentSearchEndpoint();
        group.MapPaymentUpdateEndpoint();
        group.MapPaymentVoidEndpoint();
    }
}
