using Accounting.Application.Invoices.LineItems.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Invoice.LineItems.v1;

/// <summary>
/// Endpoint for deleting an invoice line item.
/// </summary>
public static class DeleteInvoiceLineItemEndpoint
{
    /// <summary>
    /// Maps the delete invoice line item endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapDeleteInvoiceLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/line-items/{lineItemId:guid}", async (DefaultIdType lineItemId, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteInvoiceLineItemCommand(lineItemId)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteInvoiceLineItemEndpoint))
            .WithSummary("Delete invoice line item")
            .WithDescription("Deletes an invoice line item and recalculates invoice totals.")
            .Produces<DeleteInvoiceLineItemResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

