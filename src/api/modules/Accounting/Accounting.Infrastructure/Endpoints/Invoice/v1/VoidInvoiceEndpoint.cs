using Accounting.Application.Invoices.Void.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice.v1;

/// <summary>
/// Endpoint for voiding an invoice.
/// </summary>
public static class VoidInvoiceEndpoint
{
    /// <summary>
    /// Maps the void invoice endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapVoidInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/void", async (DefaultIdType id, [FromBody] VoidInvoiceCommand command, ISender mediator) =>
            {
                if (id != command.InvoiceId)
                {
                    return Results.BadRequest("Invoice ID in URL does not match the request body.");
                }

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(VoidInvoiceEndpoint))
            .WithSummary("Void an invoice")
            .WithDescription("Voids an invoice to reverse accounting impact while maintaining audit trail.")
            .Produces<VoidInvoiceResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

