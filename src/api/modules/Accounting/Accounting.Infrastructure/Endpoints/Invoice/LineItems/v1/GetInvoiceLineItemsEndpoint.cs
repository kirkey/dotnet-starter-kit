using Accounting.Application.Invoices.LineItems.Get.v1;
using Accounting.Application.Invoices.LineItems.GetList.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice.LineItems.v1;

/// <summary>
/// Endpoint for getting all line items for a specific invoice.
/// </summary>
public static class GetInvoiceLineItemsEndpoint
{
    /// <summary>
    /// Maps the get invoice line items endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapGetInvoiceLineItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{invoiceId:guid}/line-items", async (DefaultIdType invoiceId, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInvoiceLineItemsRequest(invoiceId)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetInvoiceLineItemsEndpoint))
            .WithSummary("Get all line items for an invoice")
            .WithDescription("Retrieves all line items associated with a specific invoice.")
            .Produces<List<InvoiceLineItemResponse>>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

