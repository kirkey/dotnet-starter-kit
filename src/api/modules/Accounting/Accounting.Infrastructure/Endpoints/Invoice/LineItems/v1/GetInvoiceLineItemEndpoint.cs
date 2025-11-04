using Accounting.Application.Invoices.LineItems.Get.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice.LineItems.v1;

/// <summary>
/// Endpoint for getting a specific invoice line item.
/// </summary>
public static class GetInvoiceLineItemEndpoint
{
    /// <summary>
    /// Maps the get invoice line item endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapGetInvoiceLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/line-items/{lineItemId:guid}", async (DefaultIdType lineItemId, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInvoiceLineItemRequest(lineItemId)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetInvoiceLineItemEndpoint))
            .WithSummary("Get invoice line item by ID")
            .WithDescription("Retrieves a specific invoice line item by its unique identifier.")
            .Produces<InvoiceLineItemResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

