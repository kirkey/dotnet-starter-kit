using Accounting.Application.Invoices.LineItems.Add.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice.LineItems.v1;

/// <summary>
/// Endpoint for adding a line item to an invoice.
/// </summary>
public static class AddInvoiceLineItemEndpoint
{
    /// <summary>
    /// Maps the add invoice line item endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapAddInvoiceLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{invoiceId:guid}/line-items", async (DefaultIdType invoiceId, [FromBody] AddInvoiceLineItemCommand command, ISender mediator) =>
            {
                if (invoiceId != command.InvoiceId)
                {
                    return Results.BadRequest("Invoice ID in URL does not match the request body.");
                }

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/invoices/{invoiceId}/line-items", response);
            })
            .WithName(nameof(AddInvoiceLineItemEndpoint))
            .WithSummary("Add line item to invoice")
            .WithDescription("Adds a new line item to an invoice and updates the total amount.")
            .Produces<AddInvoiceLineItemResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

