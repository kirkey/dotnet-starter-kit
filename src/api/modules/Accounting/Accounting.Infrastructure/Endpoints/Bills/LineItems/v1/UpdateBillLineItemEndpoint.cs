using Accounting.Application.Bills.LineItems.Update.v1;
using Asp.Versioning;

namespace Accounting.Infrastructure.Endpoints.Bills.LineItems.v1;

/// <summary>
/// Endpoint for updating a bill line item.
/// </summary>
public static class UpdateBillLineItemEndpoint
{
    internal static RouteHandlerBuilder MapUpdateBillLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{billId:guid}/line-items/{lineItemId:guid}", async (
                DefaultIdType billId,
                DefaultIdType lineItemId,
                UpdateBillLineItemCommand request,
                ISender mediator) =>
            {
                // Ensure IDs from route match command
                if (billId != request.BillId)
                    return Results.BadRequest("Route bill ID does not match command bill ID");

                if (lineItemId != request.LineItemId)
                    return Results.BadRequest("Route line item ID does not match command line item ID");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBillLineItemEndpoint))
            .WithSummary("Update a bill line item")
            .WithDescription("Updates an existing line item and recalculates the bill total.")
            .Produces<UpdateBillLineItemResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Bills.Edit")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

