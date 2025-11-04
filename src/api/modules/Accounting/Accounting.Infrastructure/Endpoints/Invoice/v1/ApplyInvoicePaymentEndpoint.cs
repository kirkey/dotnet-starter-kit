using Accounting.Application.Invoices.ApplyPayment.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice.v1;

/// <summary>
/// Endpoint for applying a partial payment to an invoice.
/// </summary>
public static class ApplyInvoicePaymentEndpoint
{
    /// <summary>
    /// Maps the apply payment endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapApplyInvoicePaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/apply-payment", async (DefaultIdType id, [FromBody] ApplyInvoicePaymentCommand command, ISender mediator) =>
            {
                if (id != command.InvoiceId)
                {
                    return Results.BadRequest("Invoice ID in URL does not match the request body.");
                }

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApplyInvoicePaymentEndpoint))
            .WithSummary("Apply payment to invoice")
            .WithDescription("Applies a partial payment to an invoice. Automatically marks as paid when total payments meet invoice amount.")
            .Produces<ApplyInvoicePaymentResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

