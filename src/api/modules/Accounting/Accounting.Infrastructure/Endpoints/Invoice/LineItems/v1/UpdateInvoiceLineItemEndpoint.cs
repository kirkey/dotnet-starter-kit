using Accounting.Application.Invoices.LineItems.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Invoice.LineItems.v1;

/// <summary>
/// Endpoint for updating an invoice line item.
/// </summary>
public static class UpdateInvoiceLineItemEndpoint
{
    /// <summary>
    /// Maps the update invoice line item endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdateInvoiceLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/line-items/{lineItemId:guid}", async (DefaultIdType lineItemId, [FromBody] UpdateInvoiceLineItemCommand request, ISender mediator) =>
            {
                var command = request with { LineItemId = lineItemId };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInvoiceLineItemEndpoint))
            .WithSummary("Update invoice line item")
            .WithDescription("Updates an existing invoice line item and recalculates totals.")
            .Produces<UpdateInvoiceLineItemResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

